using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.EyeTracker.Calibration;

namespace UXC.Sessions.Timeline.Results
{
    public class EyeTrackerCalibrationSessionStepResult : SessionStepResult
    {
        public EyeTrackerCalibrationSessionStepResult(IEnumerable<CalibrationResult> calibrations)
            : base(SessionStepResultType.Successful)
        {
            Calibrations = calibrations?.ToList() ?? new List<CalibrationResult>();
        }


        public List<CalibrationResult> Calibrations { get; }
    }
}
