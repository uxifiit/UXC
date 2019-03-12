/**
 * UXC.Plugins.DefaultAPI
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

namespace UXC.Plugins.DefaultAPI.Entities
{
    public class DeviceStatusInfo
    {
        public string Device { get; set; }
        public DeviceState State { get; set; } 
        public DateTime LastChange { get; set; }
    }
}
