/**
 * UXC.Plugins.SessionsAPI
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
using UXC.Sessions;
using UXC.Sessions.Recording.Local;

namespace UXC.Plugins.SessionsAPI.Recording
{
    public class SessionRecordingResults
    {
        private readonly Dictionary<string, LocalSessionRecordingResult> _results = new Dictionary<string, LocalSessionRecordingResult>(StringComparer.CurrentCultureIgnoreCase);

        public SessionRecordingResults()
        {

        }

        public void Add(string sessionId, LocalSessionRecordingResult result)
        {
            if (String.IsNullOrWhiteSpace(sessionId) == false
                && result != null)
            {
                _results.AddOrUpdate(sessionId, result, (_, __) => result);
                CurrentRecording = sessionId;
            }
        }

        public bool TryGetResult(string sessionId, out LocalSessionRecordingResult result)
        {
            result = null;

            return String.IsNullOrWhiteSpace(sessionId) == false 
                && _results.TryGetValue(sessionId, out result);
        }

        public string CurrentRecording { get; private set; }
    }
}
