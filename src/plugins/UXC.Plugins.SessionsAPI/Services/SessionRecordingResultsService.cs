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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core;
using UXC.Core.Common.Events;
using UXC.Core.Devices;
using UXC.Plugins.SessionsAPI.Recording;
using UXC.Sessions;
using UXC.Sessions.Recording.Local;

namespace UXC.Plugins.SessionsAPI.Services
{
    public class SessionRecordingResultsService
    {
        private readonly SessionRecordingResults _results;

        public SessionRecordingResultsService(SessionRecordingResults results)
        {
            _results = results;
        }


        public bool TryGetEyeGazeDataFile(out string filepath, out string filename)
        {
            return TryGetEyeGazeDataFile(_results.CurrentRecording, out filepath, out filename);
        }

        public bool TryGetEyeGazeDataFile(string sessionId, out string filepath, out string filename)
        {
            return TryGetDeviceFilePath(sessionId, DeviceType.Physiological.EYETRACKER, "data", out filepath, out filename);
        }


        public bool TryGetEyeFixationsFile(out string filepath, out string filename)
        {
            return TryGetEyeFixationsFile(_results.CurrentRecording, out filepath, out filename);
        }

        public bool TryGetEyeFixationsFile(string sessionId, out string filepath, out string filename)
        {
            return TryGetDeviceFilePath(sessionId, DeviceType.Physiological.EYETRACKER, "fixations_post", out filepath, out filename);
        }


        private bool TryGetDeviceFilePath(string sessionId, DeviceType device, string tag, out string filepath, out string filename)
        {
            LocalSessionRecordingResult result;
            if (_results.TryGetResult(sessionId, out result)
                && result.IsClosed)
            {
                filepath = result.Paths.FirstOrDefault(p => Path.GetFileNameWithoutExtension(p).Equals($"{device.Code}_{tag}", StringComparison.InvariantCultureIgnoreCase));
                if (String.IsNullOrWhiteSpace(filepath) == false
                    && File.Exists(filepath))
                {
                    filename = $"{sessionId}_{Path.GetFileName(filepath)}";
                    return true;
                }
            }

            filepath = String.Empty;
            filename = String.Empty;
            return false;
        }
    }
}
