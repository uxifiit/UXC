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
using UXC.Devices.Adapters;

namespace UXC.Core.Managers
{
    public interface IAdaptersManager : IManager<IDeviceAdapter>, IDisposable
    {
    }
}
