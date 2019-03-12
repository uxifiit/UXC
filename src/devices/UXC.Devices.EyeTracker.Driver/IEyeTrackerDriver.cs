/**
 * UXC.Devices.EyeTracker.Driver
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
using UXC.Devices.EyeTracker.Calibration;
using UXC.Core.Data;

namespace UXC.Devices.EyeTracker
{
    public delegate void GazeDataReceivedEventHandler(IEyeTrackerDriver sender, GazeData e);

    /// <summary>
    /// class representing a single tracker, based on Tobii SDK tracker. Use the manufacturer's SDK to map the manufacturer's SDK methods and events to the methods and events of this class.
    /// </summary>
    public interface IEyeTrackerDriver : IDisposable
    {
        /// <summary>
        /// Event which is fired after Gaze Data is received from the tracker.
        /// </summary>
        event GazeDataReceivedEventHandler GazeDataReceived;
        
        /// <summary>
        /// Event which is fired on connection error.
        /// </summary>
        event EventHandler<string> ConnectionError;
        
        /// <summary>
        /// Tracker manufacturer.
        /// </summary>
        string FamilyName { get; }

        /// <summary>
        /// Tracker's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Framerate at which the tracker sends gaze data.
        /// </summary>
        float Framerate { get; }

        /// <summary>
        /// ProductID of the tracker.
        /// </summary>
        string ProductId { get; }

        /// <summary>
        /// Method which should take care of connecting to this tracker.
        /// </summary>
        void Connect();

        /// <summary>
        /// Should disconnect safely from tracker.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Adds a calibration point to the tracker asynchronously.
        /// </summary>
        /// <param name="point">2D point structure</param>
        Task AddCalibrationPointAsync(Point2 point);
  
        /// <summary>
        /// Starts the calibration process which runs asynchronously.
        /// </summary>
        Task<CalibrationResult> ComputeCalibrationAsync();

        /// <summary>
        /// Starts calibration of the tracker, should be called by a Calibrator.
        /// </summary>
        /// <param name="numPoints">Number of calibration points. WARNING: can be only 5 or 9, else throws and exception</param>
        Task StartCalibrationAsync();

        /// <summary>
        /// Stops the calibration process safely.
        /// </summary>
        Task StopCalibrationAsync();

        Task SetCalibrationAsync(CalibrationData calibration);

        CalibrationData GetCalibration();


        /// <summary>
        /// Orders the tracker to START sending Gaze Data.
        /// </summary>
        void StartTracking();
        /// <summary>
        /// Orders the tracker to STOP sending Gaze Data
        /// </summary>
        void StopTracking();


        TrackBoxCoords TrackBox { get; }

        event EventHandler<TrackBoxCoords> TrackBoxChanged;


        DisplayArea DisplayArea { get; }

        event EventHandler<DisplayArea> DisplayAreaChanged;
    }
}
