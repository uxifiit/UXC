/**
 * UXC.Devices.EyeTracker.Driver.TobiiPro
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.EyeTracker.Calibration;

using UXI.Common.Helpers;
using UXC.Core.Data;
using UXI.Common;
using UXC.Core.Devices.Exceptions;
using System.Diagnostics;
using UXI.Common.Extensions;

namespace UXC.Devices.EyeTracker.TobiiPro
{
    /// <summary>
    /// Tracker implementation for Tobii trackers.
    /// </summary>
    public class TobiiProTracker : DisposableBase, IEyeTrackerDriver
    {
        private Tobii.Research.IEyeTracker _tracker;
        private Tobii.Research.ScreenBasedCalibration _calibration;

        public TobiiProTracker(Tobii.Research.IEyeTracker tracker)
        {
            _tracker = tracker;
        }

        public string FamilyName => "Tobii Pro";

        public string Name => _tracker.DeviceName;

        public string ProductId => _tracker.SerialNumber;

        public float Framerate { get; private set; }


        public event EventHandler<string> ConnectionError;
        public event GazeDataReceivedEventHandler GazeDataReceived;


        private TrackBoxCoords trackBox;
        public TrackBoxCoords TrackBox
        {
            get { return trackBox; }
            private set
            {
                var previous = trackBox;
                if (previous != value)
                {
                    trackBox = value;
                    TrackBoxChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<TrackBoxCoords> TrackBoxChanged;


        private DisplayArea displayArea;
        public DisplayArea DisplayArea
        {
            get { return displayArea; }
            private set
            {
                var previous = displayArea;
                if (previous != value)
                {
                    displayArea = value;
                    DisplayAreaChanged?.Invoke(this, value);
                }
            }
        }
        public event EventHandler<DisplayArea> DisplayAreaChanged;


        public void Connect()
        {
            try
            {
                AttachEventHandlers(_tracker);

                Framerate = _tracker.GetGazeOutputFrequency();
                TrackBox = _tracker.GetTrackBox()?.ToTrackBoxCoords();
                DisplayArea = _tracker.GetDisplayArea()?.ToDisplayArea();
            }
            catch (Tobii.Research.InsufficientLicenseException)
            { }
            catch (Tobii.Research.ConnectionFailedException)
            { }
            catch (Tobii.Research.EyeTrackerUnavailableException)
            { }
        }


        private void AttachEventHandlers(Tobii.Research.IEyeTracker tracker)
        {
            tracker.ConnectionLost += Tracker_ConnectionLost;
            tracker.TrackBoxChanged += Tracker_TrackBoxChanged;
            tracker.DisplayAreaChanged += Tracker_DisplayAreaChanged;
        }


        private void DetachEventHandlers(Tobii.Research.IEyeTracker tracker)
        {
            tracker.ConnectionLost -= Tracker_ConnectionLost;
            tracker.TrackBoxChanged -= Tracker_TrackBoxChanged;
            tracker.DisplayAreaChanged -= Tracker_DisplayAreaChanged;
        }


        private void Tracker_DisplayAreaChanged(object sender, Tobii.Research.DisplayAreaEventArgs e)
        {
            DisplayArea = e.DisplayArea?.ToDisplayArea();
        }


        private void Tracker_TrackBoxChanged(object sender, Tobii.Research.TrackBoxEventArgs e)
        {
            TrackBox = _tracker.GetTrackBox()?.ToTrackBoxCoords();
        }

        public void Disconnect()
        {
            var tracker = _tracker;
            _tracker = null;
            if (tracker != null)
            {
                DetachEventHandlers(tracker);
                tracker.Dispose();
            }
        }


        public void StartTracking()
        {
            if (_tracker != null)
            {
                _tracker.GazeDataReceived += Tracker_GazeDataReceived;
            }
            else
            {
                ConnectionError?.Invoke(this, "Tracker not found");
            }
        }


        public void StopTracking()
        {
            if (_tracker != null)
            {
                _tracker.GazeDataReceived -= Tracker_GazeDataReceived;
            }
        }


        #region calibration
        EventHandler<AsyncCompletedEventArgs> CreateHandler(TaskCompletionSource<bool> tcs)
        {
            return (s, e) =>
            {
                if (e.Cancelled)
                {
                    tcs.TrySetCanceled();
                }
                else if (e.Error != null)
                {
                    tcs.TrySetException(new CalibrationException("Error occurred, see inner exception for details.", e.Error));
                }

                tcs.TrySetResult(true);
            };
        }


        public bool IsCalibrating
        {
            get;
            private set;
        }



        public async Task AddCalibrationPointAsync(Point2 point)
        {
            if (IsCalibrating)
            {
                await _calibration.CollectDataAsync(point.ToNormalizedPoint2D());
            }
        }


        public async Task<CalibrationResult> ComputeCalibrationAsync()
        {
            this.ThrowIf(t => t.IsCalibrating == false, () => new InvalidOperationException("Eye tracker is not in calibrating mode."));
            
            var result = await _calibration.ComputeAndApplyAsync();
            
            var calibration = result?.ToCalibrationResult();

            return calibration;
        }


        public async Task StartCalibrationAsync()
        {
            _tracker.ThrowIfNull(() => new InvalidOperationException("Not connected to the eye tracker."));

            _calibration = new Tobii.Research.ScreenBasedCalibration(_tracker);

            await _calibration.EnterCalibrationModeAsync();

            IsCalibrating = true;
        }


        public async Task StopCalibrationAsync()
        {
            if (IsCalibrating)
            {
                var calibration = ObjectEx.GetAndReplace(ref _calibration, null);

                await calibration.LeaveCalibrationModeAsync();

                calibration.Dispose();
            }
        }


        public Task SetCalibrationAsync(CalibrationData calibration)
        {
            calibration.ThrowIf(c => c.DeviceFamilyName != FamilyName, nameof(calibration), "Calibration data is for a different family of devices.");

            _tracker.ApplyCalibrationData(new Tobii.Research.CalibrationData(calibration.Data));

            return Task.FromResult(true);
        }


        public CalibrationData GetCalibration()
        {
            var data = _tracker.RetrieveCalibrationData()?.Data;

            return data != null
                 ? new CalibrationData(FamilyName, Name, data)
                 : null;
        }


        #endregion


        private void Tracker_GazeDataReceived(object sender, Tobii.Research.GazeDataEventArgs e)
        {
            try
            {
                GazeDataReceived?.Invoke(this, e.ToGazeData());
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Something bad happened when data came from Tobii device");
                Debug.WriteLine(exception.ToString());
            }
        }


        private void Tracker_ConnectionLost(object sender, Tobii.Research.ConnectionLostEventArgs e)
        {
            ConnectionError?.Invoke(this, e.ToString());
        }


        #region IDisposable Members

        private bool _disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    Disconnect();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
