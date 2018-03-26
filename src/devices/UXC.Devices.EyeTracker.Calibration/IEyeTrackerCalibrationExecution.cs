using System;
using System.Collections.Generic;

namespace UXC.Devices.EyeTracker.Calibration
{
    public interface IEyeTrackerCalibrationExecution
    {
        IReadOnlyCollection<CalibrationResult> Calibrations { get; }

        event EventHandler<CalibrationResult> CalibrationFinished;
    }
}
