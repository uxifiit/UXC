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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Helpers;
using UXI.Common;
using System.IO;
using UXC.Core.Devices;
using UXC.Devices.Streamers.Configuration;
using System.Text.RegularExpressions;
using UXI.Common.Extensions;

namespace UXC.Devices.Streamers
{
    public class FFmpegStreamer
    {
        //private readonly ILog logger;

        private readonly IStreamersConfiguration _config;

        private FFmpegHelper _ffmpeg;

        public FFmpegStreamer(FFmpegHelper helper, IStreamersConfiguration configuration)
        {                             
            _ffmpeg = helper;
            _config = configuration;
        }
     
        protected Process Process { get; private set; }

        public bool IsRunning => Process != null && Process.HasExited == false;
     
        public bool IsDeviceConnected(string deviceName)
        {
            return String.IsNullOrWhiteSpace(deviceName) == false && _ffmpeg.GetDevicesListOutput().Any(line => line.Contains(deviceName));
        }


        public bool TryFindCompatibleDevice(FFmpegStreamType streamType, out string device)  // string preferredDevice, 
        {
            var devices = _ffmpeg.GetDevicesOfType(streamType).ToList();
            device = devices.FirstOrDefault()?.Name;

            return String.IsNullOrWhiteSpace(device) == false;
        }


        public bool StopRecording(TimeSpan waitForExitTimeout)
        {
            var process = Process;
            Process = null;
            if (process != null)
            {
                process.Exited -= process_Exited;
                process.EnableRaisingEvents = false;

                TryCloseRecordingProcess(process);

                _ffmpeg.KillProcessAsync(process, waitForExitTimeout)
                       .ContinueWith(_ => { Clear(process); process.Dispose(); })
                       .Forget();
            }
            return true;
        }


        private void TryCloseRecordingProcess(Process process)
        {
            try
            {
                process.StandardInput.WriteLine("q");
            }
            catch (Exception ex)
            {
                // TODO log exception
            }
        }


        private static string EnsureTargetPathIsUnique(string targetPath)
        {
            if (File.Exists(targetPath))
            {
                string filename = Path.GetFileNameWithoutExtension(targetPath);
                string extension = Path.GetExtension(targetPath);

                string directory = Path.GetDirectoryName(targetPath);
                var files = Directory.GetFiles(directory, $"{filename}*");
                if (files != null && files.Any())
                {
                    string uniquePath = targetPath;

                    for (int fileNumber = files.Length; fileNumber < Int32.MaxValue && File.Exists(uniquePath); fileNumber += 1)
                    {
                        uniquePath = Path.ChangeExtension(targetPath, $"{fileNumber.ToString("000")}{extension}"); 
                    }

                    return uniquePath;
                }
            }

            return targetPath;
        }


        public bool StartRecordingProcess(FFmpegStreamType streamType, string device, string targetPath, string deviceArgs = null)
        {
            device.ThrowIf(String.IsNullOrWhiteSpace, nameof(device));

            string uniqueTargetPath = EnsureTargetPathIsUnique(targetPath);

            string inputArgs = _config.FFmpegStartRecordingArgs
               .Replace("{streamType}", streamType)
               .Replace("{deviceName}", device)
               .Replace("{deviceArgs}", deviceArgs ?? String.Empty)
               .Replace("{targetPath}", String.IsNullOrWhiteSpace(uniqueTargetPath) ? "-f null -" : $"\"{uniqueTargetPath}\"");

            bool window = _config.ShowOutput;
            bool logging = window == false && _config.LogOutput;

            Process process = _ffmpeg.CreateFFmpegProcess(inputArgs, logging, window);


           // logger.Info(LogHelper.Prepare($"Starting streamer \"{device}\" with args: {inputArgs}"));

            try
            {
                process.EnableRaisingEvents = true;
                process.Exited += process_Exited;

                process.StartInfo.RedirectStandardInput = true; // so we can stop recording with writing "q" on the input

                if (process.StartInfo.RedirectStandardError)
                {
                    process.ErrorDataReceived += proc_ErrorDataReceived;
                }

                Process = process;

                process.Start();

                if (process.StartInfo.RedirectStandardError)
                {
                    process.BeginErrorReadLine();
                }

               // logger.Info(LogHelper.Prepare($"Streamer {device} started"));
                return true;
            }
            catch (Exception ex)
            {
                //logger.Error(LogHelper.Prepare($"Streamer {device} failed."), ex);
            }
            return false;
        }

        void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            // TODO LOG streamer output
            //logger.Info(e.Data);
        }

        void process_Exited(object sender, EventArgs e)
        {
            Process process = (Process)sender;
            if (Process == process)
            {
                Clear(process);
                process?.Dispose();

                ProcessExited?.Invoke(this, e);
                Process = null;
            }
        }

        public event EventHandler ProcessExited;

        public bool Clear()
        {
            var process = Process;
            Process = null;

            if (process != null)
            {
                _ffmpeg.KillProcessAsync(process, TimeSpan.Zero)
                       .ContinueWith(_ => { Clear(process); process?.Dispose(); })
                       .Forget();
            }

            return true;
        }

        private void Clear(Process process)
        {
            if (process != null && process.StartInfo.RedirectStandardError)
            {
                process.CancelErrorRead();
                process.ErrorDataReceived -= proc_ErrorDataReceived;
            }
        }
    }
}
