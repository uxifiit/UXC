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

namespace UXC.Core.Devices
{
    public struct DeviceStateChange
    {
        public DeviceStateChange(DeviceState state, DateTime timestamp)
        {
            State = state;
            Timestamp = timestamp;
        }   

        public DeviceState State { get; }

        public DateTime Timestamp { get; }
    }
}
