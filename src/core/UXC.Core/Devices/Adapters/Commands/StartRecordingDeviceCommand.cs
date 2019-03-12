/**
 * UXC.Core
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
using UXC.Devices.Adapters;
using UXC.Core.Devices;

namespace UXC.Devices.Adapters.Commands
{
    internal sealed class StartRecordingDeviceCommand : SyncDeviceCommand
    {
        internal StartRecordingDeviceCommand() 
            : base(DeviceAction.StartRecording, DeviceState.Recording) { }

        protected override CommandResult Execute(IDevice device)
        {
            return Result(device.StartRecording());
        }
    }
}
