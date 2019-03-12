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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UXC.Devices.Streamers.Configuration;
using UXI.Common;
using UXI.Common.Extensions;
using UXI.Common.Helpers;

namespace UXC.Devices.Streamers
{
    public class FFmpegHelper
    {
        private readonly IFFmpegConfiguration _config;

        private readonly Regex _devicesHeaderRegex;

        public FFmpegHelper(IFFmpegConfiguration config)
        {
            _config = config;

            string typesConstraint = FFmpegStreamType.Types.Select(t => t.ToString()).Aggregate((x, y) => x + "|" + y);
            _devicesHeaderRegex = new Regex(_config.DevicesListHeaderTemplate.Replace("{streamType}", $"({typesConstraint})"), RegexOptions.CultureInvariant);
        }


        public IEnumerable<string> GetDevicesListOutput()
        {
            using (Process process = CreateFFmpegProcess(_config.EnumerateDevicesArgs, true))
            {
                if (process.Start() == false)
                {
                    //logger.Error(LogHelper.Prepare("Error starting ffmpeg process"));
                    throw new InvalidOperationException("Error starting ffmpeg process.");
                }

                string line;

                using (var reader = process.StandardError)
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        yield return line;
                    }
                }

                KillProcessAsync(process, TimeSpan.Zero).Forget();
            }
        }


        private bool TryMatchDeviceTypeHeader(string line, out string device)
        {
            var match = _devicesHeaderRegex.Match(line);

            if (match.Success && match.Groups.Count > 1)
            {
                device = match.Groups[1].Value;
                return true;
            }

            device = null;
            return false;
        }


        public IEnumerable<FFmpegDevice> GetDevicesOfType(FFmpegStreamType type)
        {
            string deviceType;

            Regex regex = new Regex("\"[^@]*?\"", RegexOptions.Compiled | RegexOptions.CultureInvariant);

            IEnumerable<FFmpegDevice> devices = GetDevicesListOutput()
                .SkipWhile(l => TryMatchDeviceTypeHeader(l, out deviceType) == false || deviceType.Equals(type) == false) // skip while the header does not match the type
                .Skip(1)     // skip the header so it is not processed twice; alternative to check for the same device type in the next step with  '|| header.Equals(type)'
                .TakeWhile(l => TryMatchDeviceTypeHeader(l, out deviceType) == false) // take while other header is not reached
                .Select(line => regex.Match(line)) 
                .Where(match => match.Success)
                .SelectMany(match => match.Captures.OfType<Capture>())
                .Select(capture => capture.Value.Trim('\"'))
                .Select(name => new FFmpegDevice(type, name));

            return devices;
        }


        public Process CreateFFmpegProcess(string inputArgs, bool redirectError = false, bool showWindow = false)
        {
            var process = new Process();

            string path = Path.Combine(Locations.ExecutingAssemblyLocationPath, _config.FFmpegPath);

            var startInfo = new ProcessStartInfo(path, inputArgs)
            {
                UseShellExecute = !redirectError,
                RedirectStandardError = redirectError,
            };

            if (showWindow)
            {
                startInfo.CreateNoWindow = false;
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
            }
            else
            {
                startInfo.CreateNoWindow = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }

            process.StartInfo = startInfo;

            return process;
        }


        public async Task KillProcessAsync(Process process, TimeSpan waitForExitTimeout)
        {
            if (process != null)
            {
                if (process.HasExited == false && waitForExitTimeout > TimeSpan.Zero)
                {
                    await TryWaitForExitAsync(process, waitForExitTimeout);
                }

                if (process.HasExited == false)
                {
                    process.Kill();
                }
            }
        }


        private async static Task TryWaitForExitAsync(Process process, TimeSpan waitForExitTimeout)
        {
            try
            {
                int timeoutMilliseconds = (int)Math.Round(waitForExitTimeout.TotalMilliseconds);
                await Task.Run(() => process.WaitForExit(timeoutMilliseconds));
            }
            catch (Exception ex)
            {
                // TODO log exception
            }
        }
    }
}
