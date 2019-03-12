/**
 * UXC.Devices.EyeTracker
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace UXC.Devices.EyeTracker.Calibration
//{
//    public class CalibrationResultValidated
//    {
//        internal CalibrationResultValidated(CalibrationResult result)
//        {

//        }

//        public CalibrationValidationStatus TrackerValidationStatus { get; }




//        public const double SUCCESSFUL_VALIDITY_THRESHOLD = 0.9d;
//        public const double SUCCESS_DISTANCE_LIMIT = 0.075d; // TODO const really?

//        public void Validate(IEnumerable<CalibrationPointResult> points, double distanceLimit = SUCCESS_DISTANCE_LIMIT, double validityThreshold = SUCCESSFUL_VALIDITY_THRESHOLD)
//        {
//            if (points.Any())
//            {
//                int leftValidSamples = 0;
//                int rightValidSamples = 0;
//                int totalSamples = 0;

//                foreach (var point in points)
//                {
//                    foreach (var sample in point.Samples)
//                    {
//                        if (sample.LeftEye.Status == CalibrationSampleStatus.ValidAndUsedInCalibration
//                            && sample.LeftEye.Point.DistanceFrom(point.TruePosition) < distanceLimit)
//                        {
//                            leftValidSamples += 1;
//                        }

//                        if (sample.RightEye.Status == CalibrationSampleStatus.ValidAndUsedInCalibration
//                            && sample.RightEye.Point.DistanceFrom(point.TruePosition) < distanceLimit)
//                        {
//                            rightValidSamples += 1;
//                        }

//                        totalSamples += 1;
//                    }
//                }

//                LeftValidity = leftValidSamples / (double)totalSamples;
//                RightValidity = rightValidSamples / (double)totalSamples;

//                if (Status == CalibrationStatus.Unknown)
//                {
//                    if (LeftValidity >= validityThreshold && RightValidity >= validityThreshold)
//                    {
//                        Status = CalibrationStatus.Success;
//                    }
//                    else if (LeftValidity >= validityThreshold)
//                    {
//                        Status = CalibrationStatus.SuccessLeftEye;
//                    }
//                    else if (RightValidity >= validityThreshold)
//                    {
//                        Status = CalibrationStatus.SuccessRightEye;
//                    }
//                    else
//                    {
//                        Status = CalibrationStatus.Failure;
//                    }
//                }
//            }
//        }
//    }
//}
