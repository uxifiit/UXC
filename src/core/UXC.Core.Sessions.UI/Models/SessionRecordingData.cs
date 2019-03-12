/**
 * UXC.Core.Sessions.UI
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

namespace UXC.Sessions.Models
{
    public class SessionRecordingData
    {
        public string Project { get; set; }

        public string Name { get; set; }

        public string Source { get; set; }

        public string Path { get; set; }

        public DateTime StartTime { get; set; }
       
        public DateTime EndTime { get; set; }

        public SessionRecordingData Clone()
        {
            return (SessionRecordingData)this.MemberwiseClone();
        }
    }
}
