using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UXC.Devices.EyeTracker.Calibration;
using UXI.Common.Extensions;
using UXI.Common.Helpers;
using UXI.Common;
using UXC.Core.Common.Events;
using UXC.Core.Devices;
using UXC.Core.Devices.Calibration;
using UXI.Configuration;
using System.IO;
using UXC.Core.Data;
using UXC.Core.Configuration;
using UXC.Core.Devices.Exceptions;

namespace UXC.Devices.EyeTracker
{
    public sealed partial class EyeTrackerDevice : IDevice, IConfigurable
    {
        internal static class LogTags
        {
            public const string Calibration = "Calibration";
            public const string Connection = "Connection";
            public const string Recording = "Recording";
            public const string Info = "Info";
        }


        private readonly EyeTrackerRecordingConfiguration _config;
        private readonly TrackerBrowser _browser;

        private IEyeTrackerDriver _connectedTracker;

        public EyeTrackerDevice(TrackerBrowser browser)
        {
            _config = new EyeTrackerRecordingConfiguration();
            _browser = browser;

            Code = DeviceCode.Create(this, DeviceType.Physiological.EYETRACKER)
                .RequiresCalibrationBeforeStart(true)
                .ConnectionType(DeviceConnectionType.Port)
                .RunsOnMainThread(false)
                .Build();
        }


        private void Tracker_ConnectionError(object sender, string message)
        {
            ConnectionError?.Invoke(this, new ConnectionException(message));

            Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Error, LogTags.Calibration, message));
        }


        void Tracker_GazeDataReceived(IEyeTrackerDriver sender, GazeData data)
        {
            Data?.Invoke(this, data);
        }


        public event DeviceDataReceivedEventHandler Data;
        public event ErrorOccurredEventHandler ConnectionError;

        public event DeviceLogEventHandler Log;

        public DeviceCode Code { get; }

        public Type DataType => typeof(GazeData);


        public EyeTrackerDeviceInfo DeviceInfo { get; } = new EyeTrackerDeviceInfo();


        public bool ConnectToDevice()
        {
            try
            {
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, "Searching for an eye tracker..."));

                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                {
                    var tracker = _browser.SearchAsync(cts.Token)
                                          .ConfigureAwait(false)
                                          .GetAwaiter()
                                          .GetResult();

                    if (tracker != null)
                    {
                        string name = tracker.Name ?? String.Empty;
                        string productId = tracker.ProductId ?? String.Empty;
                        string familyName = tracker.FamilyName ?? String.Empty;

                        Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, $"Eye tracker found: {familyName} - {name} ({productId}). Connecting..."));

                        AttachEventHandlers(tracker);

                        tracker.Connect();

                        DeviceInfo.Name = name;
                        DeviceInfo.FamilyName = familyName;
                        DeviceInfo.ProductId = productId;

                        _connectedTracker = tracker;

                        Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, "Connection successful"));
                        OnCanCalibrateChanged();

                        return true;
                    }
                }
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Error, LogTags.Connection, "No eye tracker found."));
            }
            catch (OperationCanceledException)
            {
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Error, LogTags.Connection, "Failed to find an eye tracker within the time limit."));
            }
            catch (Exception ex)
            {
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Error, LogTags.Connection, "Failed to connect to an eye tracker.", ex));
                ConnectionError?.Invoke(this, new ConnectionException("Failed to connect to an eye tracker, see inner exception for details.", ex));
            }

            return false;
        }


        private void AttachEventHandlers(IEyeTrackerDriver tracker)
        {
            tracker.ConnectionError += Tracker_ConnectionError;
            tracker.TrackBoxChanged += Tracker_TrackBoxChanged;
            tracker.DisplayAreaChanged += Tracker_DisplayAreaChanged;
        }


        private void DetachEventHandlers(IEyeTrackerDriver tracker)
        {
            tracker.ConnectionError -= Tracker_ConnectionError;
            tracker.TrackBoxChanged -= Tracker_TrackBoxChanged;
            tracker.DisplayAreaChanged -= Tracker_DisplayAreaChanged;
        }


        private void Tracker_DisplayAreaChanged(object sender, DisplayArea displayArea)
        {
            Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Info, "Display area changed", displayArea));
        }


        public void DumpInfo()
        {
            var tracker = _connectedTracker;
            if (tracker != null)
            {
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Info, $"Device name: {tracker.Name}"));
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Info, $"ProductId: {tracker.ProductId}"));
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Info, $"Manufacturer: {tracker.FamilyName}"));
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Info, "TrackBox", tracker.TrackBox));
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Info, "DisplayArea", tracker.DisplayArea));
            }
            else
            {
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, "Disconnected"));
            }
        }


        public bool DisconnectDevice()
        {
            var tracker = ObjectEx.GetAndReplace(ref _connectedTracker, null);
            if (tracker != null)
            {
                DetachEventHandlers(tracker);

                tracker.Dispose();

                OnCanCalibrateChanged();
            }
            return true;
        }


        void Tracker_TrackBoxChanged(object sender, TrackBoxCoords trackBox)
        {
            DeviceInfo.TrackBox = trackBox;
            Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Info, "TrackBox changed", trackBox));
        }


        public bool StartRecording()
        {
            var tracker = _connectedTracker;
            if (tracker != null)
            {
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Recording, "Starting recording..."));

                tracker.GazeDataReceived += Tracker_GazeDataReceived;
                tracker.StartTracking();

                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Recording, "Recording started"));

                return true;
            }
            return false;
        }


        public bool StopRecording()
        {
            var tracker = _connectedTracker;
            if (tracker != null)
            {
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Recording, "Stopping recording..."));

                tracker.StopTracking();
                tracker.GazeDataReceived -= Tracker_GazeDataReceived;

                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Recording, "Recording stopped"));
                return true;
            }
            return false;
        }


        public IConfiguration Configuration => _config;
    }
}
