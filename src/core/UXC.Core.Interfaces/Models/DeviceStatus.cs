/**
 * UXC.Core.Interfaces
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;

namespace UXC.Core.Models
{
    public struct DeviceStatus
    {
        public DeviceType DeviceType { get; }
        public DeviceState State { get; }
        public DateTime LastChange { get; set; }

        public DeviceStatus(DeviceType type, DeviceState state, DateTime lastChange)
        {
            DeviceType = type;
            State = state;
            LastChange = lastChange;
        }

        public DeviceStatus Update(DeviceState state, DateTime lastChange)
        {
            return new DeviceStatus(this.DeviceType, state, lastChange);
        }
    }

    public class DeviceStatusEventArgs : EventArgs
    {
        public DeviceStatus Status { get; }
        public DeviceStatusEventArgs(DeviceStatus status)
        {
            Status = status;
        }
    }
}
