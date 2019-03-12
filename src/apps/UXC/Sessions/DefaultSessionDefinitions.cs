/**
 * UXC
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common;
using UXC.Sessions;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;
using UXC.Core.Data;
using UXC.Core.Managers;
using UXC.Models.Contexts;
using UXC.Core.Devices;
using UXI.Common.Extensions;
using System.Threading;

namespace UXC.Sessions
{
    class DefaultSessionDefinitions : ISessionDefinitionsSource
    {
        private readonly IDevicesContext _devices;

        public DefaultSessionDefinitions(IDevicesContext devices)
        {
            _devices = devices;
        }


        private void devices_DevicesUpdated(object sender, Core.Models.DeviceStatus e)
        {
            TryUpdateDefaultDefinition();
        }


        private SessionDefinition defaultDefinition;
        public SessionDefinition DefaultDefinition
        {
            get { return defaultDefinition; }
            private set
            {
                var previous = ObjectEx.GetAndReplace(ref defaultDefinition, value);
                if (previous != value)
                {
                    DefinitionsChanged?.Invoke(this, CollectionChangedEventArgs<SessionDefinition>.Create(value, previous));
                }
            }
        }


        private void TryUpdateDefaultDefinition()
        {
            var deviceTypes = _devices.Devices.Select(device => device.DeviceType).ToList();

            if (DefaultDefinition == null 
                || DefaultDefinition.SelectedDeviceTypes.Intersect(deviceTypes).Count() != deviceTypes.Count)
            {
                DefaultDefinition = CreateDefaultDefinition(deviceTypes);
            }
        }


        private static SessionDefinition CreateDefaultDefinition(IEnumerable<DeviceType> deviceTypes)
        {
            var definition = SessionDefinition.Create();
            definition.Devices = deviceTypes.Select(type => new SessionDeviceDefinition(type, null)).ToList();
            definition.Project = "Default";
            definition.Source = "Local";
            definition.Name = "Recording";
            definition.SerializationFormat = "JSON";
            definition.StrictStart = true;

            definition.Welcome.Ignore = true;

            definition.Recorders.Add(new SessionRecorderDefinition("Local"));
            definition.Recorders.Add(new SessionRecorderDefinition("Buffer"));

            bool hasEyeTracker = definition.SelectedDeviceTypes.Any(device => device.Equals(DeviceType.Physiological.EYETRACKER));

            if (hasEyeTracker)
            {
                definition.PreSessionSteps.Add(new SessionStep() { Action = new EyeTrackerCalibrationActionSettings() });
                definition.SessionSteps.Add(new SessionStep() { Action = new EyeTrackerValidationActionSettings() { PointDuration = 2000 }, Completion = new SessionStepCompletionSettings() { Hotkeys = new List<Hotkey>() { Hotkey.F10 } } });
            }

            definition.SessionSteps.Add(new SessionStep() { Action = new ShowDesktopActionSettings() { MinimizeAll = false }, Completion = new SessionStepCompletionSettings() { Hotkeys = new List<Hotkey>() { Hotkey.F10 } } });

            if (hasEyeTracker)
            {
                definition.PostSessionSteps.Add(new SessionStep() { Action = new FixationFilterActionSettings() });
            }

            return definition;
        }


        public IEnumerable<SessionDefinition> Definitions
        {
            get
            {
                if (DefaultDefinition != null)
                {
                    yield return DefaultDefinition;
                }
                yield break;
            }
        }


        public Task RefreshAsync(CancellationToken cancellationToken)
        {
            _devices.DeviceAdded -= devices_DevicesUpdated;
            _devices.DeviceRemoved -= devices_DevicesUpdated;

            if (cancellationToken.IsCancellationRequested == false)
            {
                TryUpdateDefaultDefinition();
            }

            _devices.DeviceAdded += devices_DevicesUpdated;
            _devices.DeviceRemoved += devices_DevicesUpdated;

            return Task.FromResult(true);
        }


        public event EventHandler<CollectionChangedEventArgs<SessionDefinition>> DefinitionsChanged;
    }
}
