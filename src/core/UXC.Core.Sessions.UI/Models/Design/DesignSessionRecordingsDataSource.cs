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

namespace UXC.Sessions.Models.Design
{
    public class DesignSessionRecordingsDataSource : ISessionRecordingsDataSource
    {
        private static readonly List<SessionRecordingData> Recordings = new List<SessionRecordingData>()
        {
            new SessionRecordingData() {
                Project = "New infrastructure UX case study",
                Name = "Session 1",
                Source = "Local",
                Path = @"C:\Users\ux\AppData\Local\UXC\Pilot infrastructure",
                StartTime = new DateTime(2017, 4, 20, 14, 03, 00),
            },
            new SessionRecordingData()
            {
                Project = "Pilot study",
                Name = "Session 1",
                Source = "Local",
                Path = @"C:\Users\ux\AppData\Local\UXC\Session 2",
                StartTime = new DateTime(2017, 3, 31, 16, 03, 00),
            },
            new SessionRecordingData()
            {
                Project = "Testing experiment",
                Name = "Testing session",
                Source = "Local",
                Path = @"C:\Users\ux\AppData\Local\UXC\Session 1",
                StartTime = new DateTime(2017, 3, 20, 14, 03, 00),
            },
        };

        public IEnumerable<SessionRecordingData> Load()
        {
            return Recordings;
        }

        public bool Save(IEnumerable<SessionRecordingData> recordings)
        {
            return true;
        }
    }
}
