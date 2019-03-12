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
using UXI.Common.Helpers;
using UXC.Core.Devices;
using UXC.Devices.Streamers.Configuration;
using UXI.Configuration;
using UXC.Core.Configuration;
using UXC.Core.Devices.Exceptions;
using UXC.Core.Data;
using System.IO;

namespace UXC.Devices.Streamers
{
    public class VideoStreamerDevice : IDevice, IConfigurable
    {
        private readonly FFmpegStreamer _streamer;
        private readonly IVideoStreamerConfiguration _config;
        private readonly VideoStreamerRecordingConfiguration _recordingConfig;

        internal static class LogTags
        {
            public const string Calibration = "Calibration";
            public const string Connection = "Connection";
            public const string Recording = "Recording";
            public const string Info = "Info";
        }


        private string _selectedDevice;

        public VideoStreamerDevice(FFmpegStreamer streamer, IVideoStreamerConfiguration configuration)
        {
            _config = configuration;
            _recordingConfig = new VideoStreamerRecordingConfiguration(configuration);

            _streamer = streamer;
            _streamer.ProcessExited += (_, __) => ConnectionError?.Invoke(this, new ConnectionException("Webcam video process exited."));

            Code = DeviceCode.Create(this, DeviceType.Streaming.WEBCAM_VIDEO)
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


    //public class VideoStreamAdapter : StreamAdapter<VideoConfiguration>
    //{
    //    private static readonly ILog logger = LogManager.GetLogger(typeof(VideoStreamAdapter));

    //    private int _fps = VideoConfiguration.DEFAULT_SD_FPS;
    //    private int _quality = VideoConfiguration.DEFAULT_SD_BITRATE;
    //    private string _videoResolution = VideoConfiguration.DEFAULT_SD_RESOLUTION;

    //    static VideoStreamAdapter()
    //    {
    //        log4net.Config.XmlConfigurator.Configure();
    //    }

    //    public VideoStreamAdapter(IStreamersAdatersConfiguration configuration) : base(DeviceType.WEBCAM_VIDEO, configuration, logger) { }

    //    internal VideoStreamAdapter(DeviceType device, IStreamersAdatersConfiguration configuration, ILog logger) : base(device, configuration, logger) { }

    //    protected override string DefaultRecordingDevice
    //    {
    //        get
    //        {
    //            return Configuration.Video_StreamDevice;
    //        }
    //    }

    //    protected override StreamData StreamType
    //    {
    //        get { return StreamData.Video; }
    //    }

    //    protected override bool Configure(VideoConfiguration configuration)
    //    {
    //        base.Configure(configuration);

    //        //_fps = configuration.FramesPerSecond.GetValueOrDefault(DefaultConfiguration.FramesPerSecond.Value);
    //        logger.Info(LogHelper.Prepare("StreamQuality: " + configuration.StreamQuality.ToString()));

    //        switch (configuration.StreamQuality)
    //        {
    //            case StreamQuality.Low:
    //                _videoResolution = VideoConfiguration.DEFAULT_LOW_RESOLUTION;
    //                _quality = VideoConfiguration.DEFAULT_LOW_BITRATE;
    //                _fps = VideoConfiguration.DEFAULT_LOW_FPS;
    //                break;
    //            case StreamQuality.HD:
    //                _videoResolution = VideoConfiguration.DEFAULT_HD_RESOLUTION;
    //                _quality = VideoConfiguration.DEFAULT_HD_BITRATE;
    //                _fps = VideoConfiguration.DEFAULT_HD_FPS;
    //                break;
    //            case StreamQuality.HDR:
    //                _videoResolution = VideoConfiguration.DEFAULT_HDR_RESOLUTION;
    //                _quality = VideoConfiguration.DEFAULT_HDR_BITRATE;
    //                _fps = VideoConfiguration.DEFAULT_HDR_FPS;
    //                break;
    //            default:
    //            case StreamQuality.SD:
    //                _videoResolution = VideoConfiguration.DEFAULT_SD_RESOLUTION;
    //                _quality = VideoConfiguration.DEFAULT_SD_BITRATE;
    //                _fps = VideoConfiguration.DEFAULT_SD_FPS;
    //                break;
    //        }

    //        logger.Info(LogHelper.Prepare(String.Format("Resolution: {0}, Quality: {1}, FPS: {2}", _videoResolution, _quality, _fps)));

    //        if (configuration.FramesPerSecond.HasValue)
    //        {
    //            _fps = configuration.FramesPerSecond.Value;
    //            logger.Info(LogHelper.Prepare("FPS set to: " + _fps));
    //        }

    //        if (configuration.BitrateQuality.HasValue)
    //        {
    //            _quality = configuration.BitrateQuality.Value;
    //            logger.Info(LogHelper.Prepare("Bitrate set to: " + _quality));
    //        }

    //        return _quality > 0 && _fps > 0;
    //    }


    //    protected override string BuildSpecificFFmpegArgs()
    //    {
    //        //"-s {0} -b:v {1}k -r {2} -vcodec libx264 -an -preset ultrafast "
    //        return String.Format(Configuration.Video_FFmpeg_Codec,
    //            _videoResolution,
    //            _quality,
    //            _fps);
    //    }

    //    private static readonly VideoConfiguration _defaultConfiguration = new VideoConfiguration() { BitrateQuality = VideoConfiguration.DEFAULT_SD_BITRATE, FramesPerSecond = VideoConfiguration.DEFAULT_SD_FPS, StreamQuality = StreamQuality.SD, TargetUrl = "127.0.0.1:6666" };
    //    public override VideoConfiguration DefaultConfiguration
    //    {
    //        get { return _defaultConfiguration; }
    //    }

        

    //    public override IEnumerable<string> GetCompatibleDevices()
    //    {
    //        var header_audio = Configuration.Audio_FFmpeg_DevicesListHeader;
    //        var header_video = Configuration.Video_FFmpeg_DevicesListHeader;

    //        Regex regex = new Regex("\"[^@]*?\"", RegexOptions.Compiled | RegexOptions.CultureInvariant);
    //        IEnumerable<string> devices = FFmpegHelper.GetDevicesListOutput()
    //            .SkipWhile(l => l.Contains(header_video) == false)
    //            .TakeWhile(l => l.Contains(header_audio) == false)
    //            .Select(line => regex.Match(line))
    //            .Where(match => match.Success)
    //            .SelectMany(match => match.Captures.OfType<Capture>())
    //            .Select(capture => capture.Value.Trim('\"'));

    //        return devices;
    //    }
}
