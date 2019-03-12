/**
 * UXC.Core
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
using UXC.Devices.Adapters;
using UXC.Core.Devices;
using UXC.Core.Models;
using UXC.Models.Contexts;
using UXC.Observers;
using System.Collections.Concurrent;

namespace UXC
{
    internal class DevicesContext : IDevicesContext
    {
        private ConcurrentDictionary<DeviceType, DeviceStatus> _devices = new ConcurrentDictionary<DeviceType, DeviceStatus>();

        public IEnumerable<DeviceStatus> Devices { get { return _devices.Values; } }

        public event EventHandler<DeviceStatus> DeviceUpdated;
        public event EventHandler<DeviceStatus> DeviceAdded;
        public event EventHandler<DeviceStatus> DeviceRemoved;


        public bool TryAddDevice(DeviceType device, DeviceState state, out DeviceStatus status)
        {
            status = new DeviceStatus(device, state, DateTime.Now);

            if (_devices.TryAdd(device, status))
            {
                DeviceAdded?.Invoke(this, status);
                return true;
            }

            return false;
        }

        public bool TryRemoveDevice(DeviceType device, out DeviceStatus status)
        {
            if (_devices.TryRemove(device, out status))
            {
                DeviceRemoved?.Invoke(this, status);
                return true;
            }
            return false;
        }

        public bool TryUpdateDevice(DeviceType device, DeviceState state, out DeviceStatus status)
        {
            if (_devices.TryGetValue(device, out status) && status.State != state)
            {
                status = status.Update(state, DateTime.Now);
                _devices[device] = status;

                DeviceUpdated?.Invoke(this, status);
                return true;
            }

            return false;
        }
    }
}
