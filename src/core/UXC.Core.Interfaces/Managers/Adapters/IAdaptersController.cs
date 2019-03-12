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
using System.Threading;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXC.Models;

namespace UXC.Core.Managers.Adapters
{
    public interface IAdaptersControl
    {
        //event EventHandler<UserDeviceActionRequestedEventArgs> UserDeviceActionRequired;

        //Task CheckDeviceAsync(DeviceType device, DeviceState state);
        Task ConnectAsync(CancellationToken cancellationToken);

        Task ConnectAsync(IEnumerable<DeviceType> devices, CancellationToken cancellationToken);
        Task DisconnectAsync(CancellationToken cancellationToken);
        void ResetConfigurations();
        void Configure(IDictionary<DeviceType, IDictionary<string, object>> configurations);
        Task StartRecordingAsync(CancellationToken cancellationToken);
        Task StartRecordingAsync(IEnumerable<DeviceType> devices, CancellationToken cancellationToken);
        Task StopRecordingAsync(CancellationToken cancellationToken);

        bool CheckAreDevicesInState(DeviceState state);
        bool CheckAreDevicesInState(DeviceState state, IEnumerable<DeviceType> setup);
    }
}
