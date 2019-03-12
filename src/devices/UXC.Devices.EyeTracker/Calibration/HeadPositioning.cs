/**
 * UXC.Devices.EyeTracker
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
using UXC.Core.Data;
using UXC.Devices.EyeTracker.Extensions;

namespace UXC.Devices.EyeTracker.Calibration
{
    public class HeadPositioning
    {
        private readonly IEyeTrackerDriver _tracker;

        internal HeadPositioning(IEyeTrackerDriver tracker)
        {
            _tracker = tracker;
        }


        public void Start()
        {
            _tracker.GazeDataReceived += tracker_GazeDataReceived;

            _tracker.StartTracking();
        }


        public void Stop()
        {
            _tracker.GazeDataReceived -= tracker_GazeDataReceived;

            _tracker.StopTracking();
        }


        private static DistanceRecommendation GetRecommendation(double? relativeEyeDistance)
        {
            if (relativeEyeDistance.HasValue)
            {
                double distance = relativeEyeDistance.Value;
                if (distance < 0d)
                {
                    return DistanceRecommendation.TooClose;
                }
                else if (distance > 1d)
                {
                    return DistanceRecommendation.TooFar;
                }

                return DistanceRecommendation.Alright;
            }

            return DistanceRecommendation.Unknown;
        }


        private void tracker_GazeDataReceived(IEyeTrackerDriver sender, GazeData gaze)
        {
            double? distance = gaze.GetEyeDistance();

            double? relativeEyeDistance = gaze.GetRelativeEyeDistance();
            var recommendation = GetRecommendation(relativeEyeDistance);

            // Point (0,0,0) of the TrackBox Coordinate System is in the top right corner nearest to the eye tracker from the user's
            // point of view (see Figure 3, Tobii Analytics SDK Developers Guide, p. 15), not the top left corner like the screen is,
            // so we swap the X coordinate. Y coordinate is still in the right direction. 
            var leftEyePosition = new Point2(1 - gaze.LeftEye.EyePosition3DRelative.X, gaze.LeftEye.EyePosition3DRelative.Y);
            var rightEyePosition = new Point2(1 - gaze.RightEye.EyePosition3DRelative.X, gaze.RightEye.EyePosition3DRelative.Y);

            PositionChanged?.Invoke(this, new EyesPosition(leftEyePosition, gaze.Validity.HasLeftEye(),
                                                     rightEyePosition, gaze.Validity.HasRightEye(),
                                                     distance, relativeEyeDistance, recommendation));
        }


        public event EventHandler<EyesPosition> PositionChanged;
    }
}
