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
using UXI.Common;
using UXC.Core.Common.Events;
using UXC.Core.Devices;

namespace UXC.Devices.Adapters
{
    public class DeviceStateChangedEventArgs : ValueChangedEventArgs<DeviceState>
    {
        public DeviceStateChangedEventArgs(DeviceState current, DeviceState previous, DateTime timestamp)
            : base(current, previous, timestamp)
        { }

        public DeviceStateChangedEventArgs(DeviceState state, DeviceState previous)
            : base(state, previous) 
        { }

    }
}
