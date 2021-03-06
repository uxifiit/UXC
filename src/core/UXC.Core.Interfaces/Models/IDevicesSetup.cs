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

namespace UXC.Models
{
    public interface IDevicesSetup
    {
        IEnumerable<DeviceType> Devices { get; }
        IDictionary<DeviceType, IDictionary<string, object>> Configurations { get; }
    }
}
