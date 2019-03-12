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
using System.Diagnostics;
using System.Text.RegularExpressions;
using UXC.Core.Devices;
using UXI.Configuration;
using UXC.Devices.Streamers.Configuration;
using UXC.Core.Configuration;
using UXC.Core.Devices.Exceptions;
using UXC.Core.Data;
using System.IO;

/// Poznamka: Vyzaduje instalaciu http://www.videohelp.com/tools/UScreenCapture
namespace UXC.Devices.Streamers
{
    public class ScreenCastStreamerDevice : IDevice, IConfigurable
    {
        private readonly FFmpegStreamer _streamer;
        private readonly IScreenCastStreamerConfiguration _config;
        private readonly VideoStreamerRecordingConfiguration _recordingConfig;

        internal static class LogTags
        {
            public const string Calibration = "Calibration";
            public const string Connection = "Connection";
            public const string Recording = "Recording";
            public const string Info = "Info";
        }

        private string _selectedDevice;

        public ScreenCastStreamerDevice(FFmpegStreamer streamer, IScreenCastStreamerConfiguration configuration)
        {
            _config = configuration;
            _recordingConfig = new VideoStreamerRecordingConfiguration(configuration);

            _streamer = streamer;
            _streamer.ProcessExited += (_, e) => ConnectionError?.Invoke(this, new ConnectionException("Screencast process exited."));

            Code = DeviceCode.Create(this, DeviceType.Streaming.SCREENCAST)
                .RunsOnMainThread(false)
                .ConnectionType(DeviceConnectionType.Process)
                .Build();
        }
        public DeviceCode Code { get; }

        public Type DataType { get; } = typeof(DeviceData);

        public IConfiguration Configuration => _recordingConfig;

        public event DeviceDataReceivedEventHandler Data { add { } remove { } }
        public event ErrorOccurredEventHandler ConnectionError;
        public event DisconnectedEventHandler Disconnected;
        public event ConnectedEventHandler Connected;
        public event RecordingStartedEventHandler RecordingStarted;
        public event RecordingStoppedEventHandler RecordingStopped;

        public event DeviceLogEventHandler Log;

        public bool ConnectToDevice()
        {
            string deviceName = _config.DeviceName;
            bool found = false;
            try
            {
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, $"Searching for a video device with name '{deviceName}'..."));

                found = _streamer.IsDeviceConnected(deviceName);
                if (found == false && _config.AutoSelectDevice)
                {
                    Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, $"No video device with name '{deviceName}' found."));
                    Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, "Searching for compatible video device."));
                    found = _streamer.TryFindCompatibleDevice(FFmpegStreamType.VIDEO, out deviceName);
                }

                if (found)
                {
                    _selectedDevice = deviceName;

                    Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, $"Video device found: '{deviceName}'. Connection successful."));
                    Connected?.Invoke(this, EventArgs.Empty);
                    return true;
                }

                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, $"No video device found."));
            }
            catch (Exception ex)
            {
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Error, LogTags.Connection, "Failed to connect to a video device.", ex));
                ConnectionError?.Invoke(this, new ConnectionException("Failed to connect to a video device, see inner exception for details.", ex));
            }

            return false;
        }

        public bool DisconnectDevice()
        {
            _streamer.Clear();
            _selectedDevice = null;
            Disconnected?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public bool StartRecording()
        {
            VideoStreamerRecordingPreset preset = VideoStreamerRecordingPreset.Source;
            if (String.IsNullOrWhiteSpace(_recordingConfig.Preset) == false)
            {
                preset = VideoStreamerRecordingPreset.Presets.FirstOrDefault(p => p.Name == _recordingConfig.Preset) ?? preset;
            }

            string codecArgs = _config.FFmpegCodecArgs
                .Replace("{resolutionScale}", preset.ResolutionScale)
                .Replace("{bitrate}", preset.Bitrate.ToString())
                .Replace("{framerate}", preset.FrameRate.ToString());

            string targetPath = Path.ChangeExtension(_recordingConfig.TargetPath, _config.Extension);

            bool started = _streamer.StartRecordingProcess(FFmpegStreamType.VIDEO, _selectedDevice, targetPath, codecArgs);
            if (started)
            {
                RecordingStarted?.Invoke(this, EventArgs.Empty);
            }

            return started;
        }

        public bool StopRecording()
        {
            bool stopped = _recordingConfig.StopRecordingTimeoutMilliseconds > 0
                         ? _streamer.StopRecording(TimeSpan.FromMilliseconds(_recordingConfig.StopRecordingTimeoutMilliseconds))
                         : _streamer.StopRecording(TimeSpan.Zero);

            if (stopped)
            {
                RecordingStopped?.Invoke(this, EventArgs.Empty);
            }
            return stopped;
        }

        public void DumpInfo()
        {
            Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Info, $"Device name: {_selectedDevice}"));
            Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Recording, $"Recording: {_streamer.IsRunning}"));
        }
    }
}
