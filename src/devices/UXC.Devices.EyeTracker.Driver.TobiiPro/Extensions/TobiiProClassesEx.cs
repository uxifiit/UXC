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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;
using UXC.Devices.EyeTracker.Calibration;

namespace UXC.Devices.EyeTracker.TobiiPro
{
    internal static class TobiiProClassesEx
    {
        internal static CalibrationResult ToCalibrationResult(this Tobii.Research.CalibrationResult result)
        {

            var points = result.CalibrationPoints?.Where(i => i.PositionOnDisplayArea.X != 0)
                                                  .Select(p => p.ToCalibrationPointResult())
                       ?? Enumerable.Empty<CalibrationPointResult>();

            var status = result.Status.ToCalibrationStatus();

            return new CalibrationResult(points, status);
        }


        internal static CalibrationPointResult ToCalibrationPointResult(this Tobii.Research.CalibrationPoint point)
        {
            return new CalibrationPointResult
            (
                truePos: point.PositionOnDisplayArea.ToPoint2(),
                samples: point.CalibrationSamples.Select(s => s.ToCalibrationSample())
            );
        }


        internal static CalibrationSample ToCalibrationSample(this Tobii.Research.CalibrationSample sample)
        {
            return new CalibrationSample
            (
                leftEye: new CalibrationEyeSample(sample.LeftEye.PositionOnDisplayArea.ToPoint2(), sample.LeftEye.Validity.ToCalibrationSampleStatus()),
                rightEye: new CalibrationEyeSample(sample.RightEye.PositionOnDisplayArea.ToPoint2(), sample.RightEye.Validity.ToCalibrationSampleStatus())
            );
        }


        internal static DisplayArea ToDisplayArea(this Tobii.Research.DisplayArea configuration)
        {
            return new DisplayArea
            (
                bottomLeft: configuration.BottomLeft.ToPoint3(),
                topLeft: configuration.TopLeft.ToPoint3(),
                topRight: configuration.TopRight.ToPoint3()
            );
        }


        internal static TrackBoxCoords ToTrackBoxCoords(this Tobii.Research.TrackBox trackBox)
        {
            return new TrackBoxCoords()
            {
                FrontUpperRight = trackBox.FrontUpperRight.ToPoint3(),
                FrontUpperLeft = trackBox.FrontUpperLeft.ToPoint3(),
                FrontLowerLeft = trackBox.FrontLowerLeft.ToPoint3(),
                FrontLowerRight = trackBox.FrontLowerRight.ToPoint3(),
                BackUpperRight = trackBox.BackUpperRight.ToPoint3(),
                BackUpperLeft = trackBox.BackUpperLeft.ToPoint3(),
                BackLowerLeft = trackBox.BackLowerLeft.ToPoint3(),
                BackLowerRight = trackBox.BackLowerRight.ToPoint3()
            };
        }


        // see Tobii Analytics SDK Developer Guide p. 18
        internal static GazeDataValidity GetEyeValidity(Tobii.Research.Validity leftValidity, Tobii.Research.Validity rightValidity)
        {
            if (leftValidity == Tobii.Research.Validity.Valid && rightValidity == Tobii.Research.Validity.Valid)
            {
                return GazeDataValidity.Both;
            }
            else if (leftValidity == Tobii.Research.Validity.Valid && rightValidity == Tobii.Research.Validity.Invalid)
            {
                return GazeDataValidity.Left;
            }
            //else if (leftValidity == 1 && rightValidity == 3)
            //{
            //    return GazeDataValidity.ProbablyLeft;
            //}
            //else if (leftValidity == 2 && rightValidity == 2)
            //{
            //    return GazeDataValidity.UnknownWhichOne;
            //}
            //else if (leftValidity == 3 && rightValidity == 1)
            //{
            //    return GazeDataValidity.ProbablyRight;
            //}
            else if (leftValidity == Tobii.Research.Validity.Invalid && rightValidity == Tobii.Research.Validity.Valid)
            {
                return GazeDataValidity.Right;
            }

            // leftValidity == 4 && rightValidity == 4
            return GazeDataValidity.None;
        }


        internal static GazeData ToGazeData(this Tobii.Research.GazeDataEventArgs gazeDataItem)
        {
            var validity = GetEyeValidity(gazeDataItem.LeftEye.GazePoint.Validity, gazeDataItem.RightEye.GazePoint.Validity);

            var gaze = new GazeData
            (
                validity,
                new EyeGazeData
                (
                    validity.GetLeftEyeValidity(),
                    gazeDataItem.LeftEye.GazePoint.PositionOnDisplayArea.ToPoint2(),
                    gazeDataItem.LeftEye.GazePoint.PositionInUserCoordinates.ToPoint3(),
                    gazeDataItem.LeftEye.GazeOrigin.PositionInUserCoordinates.ToPoint3(),
                    gazeDataItem.LeftEye.GazeOrigin.PositionInTrackBoxCoordinates.ToPoint3(),
                    gazeDataItem.LeftEye.Pupil.PupilDiameter
                ),
                new EyeGazeData
                (
                    validity.GetRightEyeValidity(),
                    gazeDataItem.RightEye.GazePoint.PositionOnDisplayArea.ToPoint2(),
                    gazeDataItem.RightEye.GazePoint.PositionInUserCoordinates.ToPoint3(),
                    gazeDataItem.RightEye.GazeOrigin.PositionInUserCoordinates.ToPoint3(),
                    gazeDataItem.RightEye.GazeOrigin.PositionInTrackBoxCoordinates.ToPoint3(),
                    gazeDataItem.RightEye.Pupil.PupilDiameter
                ),
                gazeDataItem.DeviceTimeStamp,
                DateTime.Now // TODO derive timestamp from Tobii timestamp
            );

            return gaze;
        }
    }
}
