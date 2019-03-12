/**
 * UXC.Core.Sessions.UI
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
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.ViewModels
{
    public class CreateSessionViewModel : ISessionChoiceViewModel
    {
        public CreateSessionViewModel()
        {

        }

        public int Id => 0;

        public string Name { get; set; }

        public bool CanEditName => true;

        public string ChoiceName => "<< Create new session >>";

        public ICollection<DeviceType> SelectedDevices { get; } = new List<DeviceType>();

        public bool CanEditDevices => true;

       // public bool CanLockDevices => false;

        public SessionDefinition GetDefinition()
        {
            var definition = SessionDefinition.Create();
            definition.Name = Name;
            definition.Project = Name;
            definition.Source = "Local";

            definition.Welcome.Ignore = true;
            definition.StrictStart = true;

            definition.Recorders.Add(new SessionRecorderDefinition("Local")); 
            definition.Recorders.Add(new SessionRecorderDefinition("Buffer"));

            definition.Devices = SelectedDevices.Select(d => new SessionDeviceDefinition(d)).ToList();

            //bool hasEyeTracker = definition.SelectedDeviceTypes.Any(device => device.Equals(DeviceType.Physiological.EYETRACKER));
            //if (hasEyeTracker)
            //{
            //    definition.PreSessionSteps.Add(new SessionStep() { Action = new EyeTrackerCalibrationActionSettings() });
            //    definition.SessionSteps.Add(new SessionStep() { Action = new EyeTrackerValidationActionSettings() { PointDuration = TimeSpan.FromSeconds(1) } });
            //}

            definition.SessionSteps.Add(new SessionStep() { Action = new ShowDesktopActionSettings() { MinimizeAll = false } });

            return definition;
        }
    }
}
