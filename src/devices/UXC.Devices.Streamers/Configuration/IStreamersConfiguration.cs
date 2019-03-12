/**
 * UXC.Devices.Streamers
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using UXI.Configuration;

namespace UXC.Devices.Streamers.Configuration
{
    public interface IStreamersConfiguration
    {
        bool AutoSelectDevice { get; }
        bool LogOutput { get; }
        bool ShowOutput { get; }
        int StopRecordingTimeoutMilliseconds { get; }
        string FFmpegStartRecordingArgs { get; }
    }
}
