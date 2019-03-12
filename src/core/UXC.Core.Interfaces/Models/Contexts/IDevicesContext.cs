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
using UXC.Core.Models;

namespace UXC.Models.Contexts
{
    public interface IDevicesContext 
    {
        IEnumerable<DeviceStatus> Devices { get; }

        event EventHandler<DeviceStatus> DeviceUpdated;
        event EventHandler<DeviceStatus> DeviceAdded;
        event EventHandler<DeviceStatus> DeviceRemoved;
    }
}
