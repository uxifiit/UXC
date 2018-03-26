using System.Collections.Generic;
using UXC.Devices.EyeTracker.Calibration;

namespace UXC.Devices.EyeTracker.Models
{
    public interface ICalibrationProfilesService
    {
        IEnumerable<CalibrationInfo> GetStoredCalibrations();
        IEnumerable<CalibrationInfo> GetStoredCalibrations(string deviceFamilyName);
        CalibrationData LoadCalibration(CalibrationInfo calibration);
        bool TrySaveCalibration(string name, CalibrationData calibration, out CalibrationInfo info);
    }
}
