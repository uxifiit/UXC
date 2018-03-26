using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

using UXC.Devices;
using UXI.Common.Helpers;
using UXC.Core.Devices;
using UXC.Devices.Streamers.Configuration;
using UXI.Configuration;
using UXC.Core.Configuration;
using UXC.Core.Devices.Exceptions;
using UXC.Core.Data;

namespace UXC.Devices.Streamers
{
    public class AudioStreamerDevice : IDevice, IConfigurable
    {
        private readonly FFmpegStreamer _streamer;
        private readonly IAudioStreamerConfiguration _config;
        private readonly AudioStreamerRecordingConfiguration _recordingConfig;

        private string _selectedDevice;

        internal static class LogTags
        {
            public const string Calibration = "Calibration";
            public const string Connection = "Connection";
            public const string Recording = "Recording";
            public const string Info = "Info";
        }

        public AudioStreamerDevice(FFmpegStreamer streamer, IAudioStreamerConfiguration configuration)
        {
            _config = configuration;
            _recordingConfig = new AudioStreamerRecordingConfiguration(configuration);

            _streamer = streamer;
            _streamer.ProcessExited += (_, e) => ConnectionError?.Invoke(this, new ConnectionException("Audio process exited."));

            Code = DeviceCode.Create(this, DeviceType.Streaming.WEBCAM_AUDIO)
                .RunsOnMainThread(false)
                .ConnectionType(DeviceConnectionType.Process)
                .Build();
        }

        

        public DeviceCode Code { get; }
        public IConfiguration Configuration => _recordingConfig;

        public Type DataType { get; } = typeof(DeviceData);

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
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, $"Searching for an audio device with name '{deviceName}'..."));

                found = _streamer.IsDeviceConnected(deviceName);
                if (found == false && _config.AutoSelectDevice)
                {
                    Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, $"No audio device with name '{deviceName}' found."));
                    Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, "Searching for compatible audio device."));
                    found = _streamer.TryFindCompatibleDevice(FFmpegStreamType.AUDIO, out deviceName);
                }

                if (found)
                {
                    _selectedDevice = deviceName;

                    Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, $"Audio device found: '{deviceName}'. Connection successful."));
                    Connected?.Invoke(this, EventArgs.Empty);
                    return true;
                }

                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, $"No audio device found."));
            }
            catch (Exception ex)
            {
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Error, LogTags.Connection, "Failed to connect to an audio device.", ex));
                ConnectionError?.Invoke(this, new ConnectionException("Failed to connect to an audio device, see inner exception for details.", ex));
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
            if (String.IsNullOrWhiteSpace(_selectedDevice) == false
                && _streamer.IsRunning == false)
            {
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Recording, "Starting recording..."));

                string codecArgs = _config.FFmpegCodecArgs
                     .Replace("{bitrate}", _recordingConfig.Bitrate.ToString());

                string targetPath = Path.ChangeExtension(_recordingConfig.TargetPath, _config.Extension);

                bool started = _streamer.StartRecordingProcess(FFmpegStreamType.AUDIO, _selectedDevice, targetPath, codecArgs);
                if (started)
                {
                    RecordingStarted?.Invoke(this, EventArgs.Empty);
                    Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Recording, "Recording started"));
                }

                return started;
            }
            return false;
        }

        public bool StopRecording()
        {
            Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Recording, "Stopping recording..."));

            bool stopped = _recordingConfig.StopRecordingTimeoutMilliseconds > 0
                         ? _streamer.StopRecording(TimeSpan.FromMilliseconds(_recordingConfig.StopRecordingTimeoutMilliseconds))
                         : _streamer.StopRecording(TimeSpan.Zero);

            if (stopped)
            {
                RecordingStopped?.Invoke(this, EventArgs.Empty);
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Recording, "Recording stopped"));
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
