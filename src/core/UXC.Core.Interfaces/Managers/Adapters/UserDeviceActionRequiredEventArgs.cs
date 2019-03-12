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
using UXC.Devices.Adapters;
using UXC.Core.Devices;

namespace UXC.Core.Managers.Adapters
{
    public class UserDeviceActionRequest
    {
        public UserDeviceActionRequest(DeviceType device, DeviceState state)
        {
            Device = device;
            TargetState = state;
        }   
        public DeviceType Device { get; private set; }
        public DeviceState TargetState { get; private set; }
    }

    public class UserDeviceActionRequestedEventArgs
    {
        public IEnumerable<UserDeviceActionRequest> Requests { get; private set; }

        public UserDeviceActionRequestedEventArgs(IEnumerable<UserDeviceActionRequest> requests)
        {
            Requests = requests.ToArray();
        }
    }
}
