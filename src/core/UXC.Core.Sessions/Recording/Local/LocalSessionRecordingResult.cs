/**
 * UXC.Core.Sessions
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

namespace UXC.Sessions.Recording.Local
{
    public class LocalSessionRecordingResult : ISessionRecordingResult
    {
        internal LocalSessionRecordingResult(SessionRecording recording, string rootFolder)
        {
            Recording = recording;
            RootFolder = rootFolder;
            Paths = new List<string>();
        }

        public SessionRecording Recording { get; }

        public string RootFolder { get; }

        public ICollection<string> Paths { get; }

        public bool IsClosed { get; private set; }

        internal void Close()
        {
            IsClosed = true;
        }
    }
}
