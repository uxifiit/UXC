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
    [Flags]
    public enum DeviceState
    {
        Disconnected = 0,
       // Connecting = 1,
        Connected = 2,
        //CalibrationRequired = 6,
        //Calibrating = 14,
        //Ready = 4 | Connected,
        Recording = 4 | Connected,
        Error = 512
    }
}
