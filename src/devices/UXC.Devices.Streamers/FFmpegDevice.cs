/**
 * UXC.Devices.Streamers
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

namespace UXC.Devices.Streamers
{
    public class FFmpegDevice
    {
        public FFmpegDevice(FFmpegStreamType type, string name)
        {
            Type = type;
            Name = name;
        }

        public string Name { get; }

        public FFmpegStreamType Type { get; }
        //public string AlternativeName { get; }
    }
}
