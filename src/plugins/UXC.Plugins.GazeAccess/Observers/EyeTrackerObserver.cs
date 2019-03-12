/**
 * UXC.Plugins.GazeAccess
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
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Observers;

namespace UXC.Plugins.GazeAccess.Observers
{
    public class EyeTrackerObserver : IDeviceObserver
    {
        public EyeTrackerObserver()
        {
            Reset();
        }

        public IObservable<GazeData> Data { get; private set; }
        public IObservable<DeviceStateChange> States { get; private set; }

        public GazeData RecentData { get; private set; }
        public DeviceState RecentState { get; private set; }

        public IDisposable Connect(IObservableDevice device)
        {
            Data = device.Data.Cast<GazeData>();
            States = device.States;

            return new CompositeDisposable
            (
                Data.StartWith(GazeData.Empty).Subscribe(d => RecentData = d),
                States.Select(s => s.State).StartWith(DeviceState.Disconnected).Subscribe(s => RecentState = s)
            );
        }

        private void Reset()
        {
            Data = Observable.Empty<GazeData>();
            States = Observable.Empty<DeviceStateChange>();
            RecentData = GazeData.Empty;
            RecentState = DeviceState.Disconnected;
        }

        public bool IsDeviceSupported(DeviceType type)
        {
            return DeviceType.Physiological.EYETRACKER.Equals(type);
        }
    }
}
