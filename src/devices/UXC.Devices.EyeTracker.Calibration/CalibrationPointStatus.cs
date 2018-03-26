using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Devices.EyeTracker.Calibration
{
    public enum CalibrationSampleStatus : int
    {
        //
        // Summary:
        //     Calibration point has failed or is invalid.
        FailedOrInvalid = -1,
        //
        // Summary:
        //     Calibration point is valid but not used in calibration.
        ValidButNotUsedInCalibration = 0,
        //
        // Summary:
        //     Calibration point is valid and used in calibration.
        ValidAndUsedInCalibration = 1
    }
}
