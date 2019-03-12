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
using UXC.Core;
using UXC.Core.Devices;
using UXC.Core.Data;

namespace UXC.Devices.Adapters
{
    public interface IObservableDevice
    {
        DeviceType DeviceType { get; }

        Type DataType { get; }

        IObservable<DeviceData> Data { get; }
        IObservable<DeviceStateChange> States { get; }
        IObservable<LogMessage> Logs { get; }
        IObservable<Exception> ConnectionErrors { get; } 

        // IObservable<Trigger> Control { get; }
    }
}
