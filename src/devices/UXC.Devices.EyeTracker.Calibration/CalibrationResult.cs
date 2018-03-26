using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Devices.EyeTracker.Calibration
{
    public class CalibrationResult
    {                                               
        public CalibrationResult(IEnumerable<CalibrationPointResult> points)
        {
            Points = points?.ToList() ?? Enumerable.Empty<CalibrationPointResult>();
        }

        public CalibrationResult(IEnumerable<CalibrationPointResult> points, CalibrationStatus status)
            : this(points)
        {
            Status = status;
        }

        public IEnumerable<CalibrationPointResult> Points { get; }

        public CalibrationStatus Status { get; } = CalibrationStatus.Unknown;
    }
}
