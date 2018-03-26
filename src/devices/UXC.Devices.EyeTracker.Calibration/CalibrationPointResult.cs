using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;

namespace UXC.Devices.EyeTracker.Calibration
{
    public class CalibrationSample
    {
        public CalibrationSample(CalibrationEyeSample leftEye, CalibrationEyeSample rightEye)
        {
            LeftEye = leftEye;
            RightEye = rightEye;
        }

        public CalibrationEyeSample LeftEye { get; }

        public CalibrationEyeSample RightEye { get; }
    }


    public class CalibrationEyeSample
    {
        public CalibrationEyeSample(Point2 point, CalibrationSampleStatus status)
        {
            Point = point;
            Status = status;
        }

        public Point2 Point { get; }

        public CalibrationSampleStatus Status { get; }
    }


    public class CalibrationPointResult
    {
        public CalibrationPointResult(Point2 truePos, IEnumerable<CalibrationSample> samples)
        {
            TruePosition = truePos;
            Samples = samples?.ToList() ?? Enumerable.Empty<CalibrationSample>();
        }

        public Point2 TruePosition { get; }

        public IEnumerable<CalibrationSample> Samples { get; }
    }
}
