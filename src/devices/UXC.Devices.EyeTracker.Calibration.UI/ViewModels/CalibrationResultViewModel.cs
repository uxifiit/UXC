using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.EyeTracker.Calibration;
using UXI.Common.UI;
using UXC.Core.Data;

namespace UXC.Devices.EyeTracker.ViewModels
{
    public struct CalibrationPointReferenceLine
    {
        public CalibrationPointReferenceLine(Point2 truePosition, Point2 mapPosition)
        {
            X = truePosition.X;
            Y = truePosition.Y;

            MapRelativeX = mapPosition.X - truePosition.X;
            MapRelativeY = mapPosition.Y - truePosition.Y;
        }

        public double X { get; } 
        public double Y { get; } 

        public double MapRelativeX { get; }
        public double MapRelativeY { get; }
    }

    public class CalibrationResultViewModel : BindableBase
    {
        private readonly CalibrationPlan _plan;
        private readonly CalibrationResult _result;

        public CalibrationResultViewModel(CalibratorViewModel calibrator, CalibrationPlan plan, CalibrationResult result)
        {
            Calibrator = calibrator;
            _plan = plan;
            _result = result;

            Status = result.Status;

            Validate(result.Points);
        }

        public IEnumerable<Point2> TruePositions => _plan.Distinct();
        public IEnumerable<Point2> LeftMapPositions => _result.Points
                                                              .SelectMany(p => p.Samples)
                                                              .Where(s => s.LeftEye.Status == CalibrationSampleStatus.ValidAndUsedInCalibration)
                                                              .Select(s => s.LeftEye.Point);
        public IEnumerable<Point2> RightMapPositions => _result.Points
                                                               .SelectMany(p => p.Samples)
                                                               .Where(s => s.RightEye.Status == CalibrationSampleStatus.ValidAndUsedInCalibration)
                                                               .Select(s => s.RightEye.Point);

        public IEnumerable<CalibrationPointReferenceLine> LeftMapLines => _result.Points
                                                                                 .SelectMany(p => p.Samples
                                                                                                   .Where(s => s.LeftEye.Status == CalibrationSampleStatus.ValidAndUsedInCalibration)
                                                                                                   .Select(s => new CalibrationPointReferenceLine(p.TruePosition, s.LeftEye.Point)));

        public IEnumerable<CalibrationPointReferenceLine> RightMapLines => _result.Points
                                                                                  .SelectMany(p => p.Samples
                                                                                                    .Where(s => s.RightEye.Status == CalibrationSampleStatus.ValidAndUsedInCalibration)
                                                                                                    .Select(s => new CalibrationPointReferenceLine(p.TruePosition, s.RightEye.Point)));

        public CalibrationStatus Status { get; private set; }


        private static string ValidityToString(double value)
        {
            double truncated = Math.Truncate(value * 100) / 100;
            return truncated.ToString("0.##%");
        }



        public double LeftValidity { get; private set; }

        public double RightValidity { get; private set; }

        public string LeftValidityString => ValidityToString(LeftValidity);
        public string RightValidityString => ValidityToString(RightValidity);


        public bool IsValid => Status == CalibrationStatus.Success;


        public string RecommendationString
        {
            get
            {
                switch (Status)
                {
                    case CalibrationStatus.Success:
                        return "Successful";
                    case CalibrationStatus.SuccessLeftEye:
                        return "Succcessful for left eye only.";
                    case CalibrationStatus.SuccessRightEye:
                        return "Successful for right eye only.";
                    case CalibrationStatus.Failure:
                        return "Please, try again.";
                }
                return String.Empty;
            }
        }

        public CalibratorViewModel Calibrator { get; }


        public const double SUCCESSFUL_VALIDITY_THRESHOLD = 0.9d;
        public const double SUCCESS_DISTANCE_LIMIT = 0.075d; // TODO const really?

        public void Validate(IEnumerable<CalibrationPointResult> points, double distanceLimit = SUCCESS_DISTANCE_LIMIT, double validityThreshold = SUCCESSFUL_VALIDITY_THRESHOLD)
        {
            if (points.Any())
            {
                int leftValidSamples = 0;
                int rightValidSamples = 0;
                int totalSamples = 0;

                foreach (var point in points)
                {
                    foreach (var sample in point.Samples)
                    {
                        if (sample.LeftEye.Status == CalibrationSampleStatus.ValidAndUsedInCalibration
                            && sample.LeftEye.Point.DistanceFrom(point.TruePosition) < distanceLimit)
                        {
                            leftValidSamples += 1;
                        }

                        if (sample.RightEye.Status == CalibrationSampleStatus.ValidAndUsedInCalibration
                            && sample.RightEye.Point.DistanceFrom(point.TruePosition) < distanceLimit)
                        {
                            rightValidSamples += 1;
                        }

                        totalSamples += 1;
                    }
                }

                LeftValidity = leftValidSamples / (double)totalSamples;
                RightValidity = rightValidSamples / (double)totalSamples;

                if (Status == CalibrationStatus.Unknown)
                {
                    if (LeftValidity >= validityThreshold && RightValidity >= validityThreshold)
                    {
                        Status = CalibrationStatus.Success;
                    }
                    else if (LeftValidity >= validityThreshold)
                    {
                        Status = CalibrationStatus.SuccessLeftEye;
                    }
                    else if (RightValidity >= validityThreshold)
                    {
                        Status = CalibrationStatus.SuccessRightEye;
                    }
                    else
                    {
                        Status = CalibrationStatus.Failure;
                    }
                }
            }
        }
    }
}
