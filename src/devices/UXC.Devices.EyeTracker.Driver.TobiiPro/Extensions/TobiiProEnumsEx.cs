using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.EyeTracker.Calibration;

namespace UXC.Devices.EyeTracker.TobiiPro
{
    internal static class TobiiProEnumsEx
    {
        internal static CalibrationSampleStatus ToCalibrationSampleStatus(this Tobii.Research.CalibrationEyeValidity status)
        {
            switch (status)
            {
                case Tobii.Research.CalibrationEyeValidity.ValidAndUsed:
                    return CalibrationSampleStatus.ValidAndUsedInCalibration;
                case Tobii.Research.CalibrationEyeValidity.ValidButNotUsed:
                    return CalibrationSampleStatus.ValidButNotUsedInCalibration;
                case Tobii.Research.CalibrationEyeValidity.InvalidAndNotUsed:
                default:
                    return CalibrationSampleStatus.FailedOrInvalid;
            }
        }


        internal static CalibrationStatus ToCalibrationStatus(this Tobii.Research.CalibrationStatus status)
        {
            switch (status)
            {
                case Tobii.Research.CalibrationStatus.Success:
                    return CalibrationStatus.Success;
                case Tobii.Research.CalibrationStatus.SuccessLeftEye:
                    return CalibrationStatus.SuccessLeftEye;
                case Tobii.Research.CalibrationStatus.SuccessRightEye:
                    return CalibrationStatus.SuccessRightEye;
                case Tobii.Research.CalibrationStatus.Failure:
                default:
                    return CalibrationStatus.Failure;
            }
        }
    }
}
