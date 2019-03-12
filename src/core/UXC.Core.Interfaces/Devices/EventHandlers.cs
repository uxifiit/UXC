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
using UXC.Core.Data;

namespace UXC.Core.Devices
{
    public delegate void DeviceDataReceivedEventHandler(IDevice device, DeviceData data);

    public delegate void DisconnectedEventHandler(IDevice device, EventArgs args);
    public delegate void ConnectedEventHandler(IDevice device, EventArgs args);
    public delegate void RecordingStartedEventHandler(IDevice device, EventArgs args);
    public delegate void RecordingStoppedEventHandler(IDevice device, EventArgs args);

    public delegate void ErrorOccurredEventHandler(IDevice device, Exception exception);

    public delegate void DeviceLogEventHandler(IDevice device, LogMessage info);
}
