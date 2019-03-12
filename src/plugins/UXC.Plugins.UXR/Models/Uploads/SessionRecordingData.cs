/**
 * UXC.Plugins.UXR
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

namespace UXC.Plugins.UXR.Models.Uploads
{
    public class SessionRecordingData
    {
        public string Path { get; set; }

        public int? SessionId { get; set; }

        public string SessionName { get; set; }

        public string Project { get; set; }

        public string RecordingId { get; set; }

        public DateTime StartTime { get; set; }

        public SessionRecordingData Clone()
        {
            return (SessionRecordingData)this.MemberwiseClone();
        }
    }
}
