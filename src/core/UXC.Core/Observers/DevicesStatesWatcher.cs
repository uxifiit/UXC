/**
 * UXC.Core
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Core.Managers.Adapters;
using UXI.Common.Extensions;
using UXC.Core.Models;
using UXC.Core.Devices;
using UXC.Core.Logging;

namespace UXC.Observers
{
    public class DevicesStatesWatcher : IDeviceObserver
    {
        private const int THROTTLE_SECONDS = 3;

        private readonly DevicesContext _context;
        private readonly ILogger _logger;

        internal DevicesStatesWatcher(DevicesContext context, ILogger logger)
        {
            _logger = logger;
            _context = context;
        }   

        public event EventHandler<DeviceStatus> DeviceStateChanged;
        public event EventHandler<DeviceStatus> StartedObservingDevice;
        public event EventHandler<DeviceStatus> StoppedObservingDevice;

        private void Device_StateChanged(IObservableDevice device, DeviceState state)
        {
            _logger.Info(device.DeviceType.Code + " " + state);

            DeviceStatus status;
            if (_context.TryUpdateDevice(device.DeviceType, state, out status))
            {
                DeviceStateChanged?.Invoke(this, status);
            }
        }

        #region IEventClient Members

        public bool IsDeviceSupported(DeviceType type) => true;

        public IDisposable Connect(IObservableDevice device)
        {
            DeviceType deviceType = device.DeviceType;

            var observer = device.States
                                 //.Where(state => DeviceStatesHelper.IsBusy(state) == false)
                                 .Throttle(TimeSpan.FromSeconds(THROTTLE_SECONDS))
                                 .DistinctUntilChanged()
                                 .ObserveOnDispatcher()
                                 .Subscribe(onNext: state => Device_StateChanged(device, state.State));

            var currentState = device.States.Select(s => s.State).MostRecent(DeviceState.Disconnected).First();

            DeviceStatus status;
            if (_context.TryAddDevice(deviceType, currentState, out status))
            {
                StartedObservingDevice?.Invoke(this, status);
            }

            return new CompositeDisposable
                (
                    observer,
                    Disposable.Create(() =>
                    {
                        DeviceStatus lastStatus;
                        if (_context.TryRemoveDevice(deviceType, out lastStatus))
                        {
                            StoppedObservingDevice?.Invoke(this, lastStatus);    
                        }
                    })
                );            
        }

        #endregion
    }
}
