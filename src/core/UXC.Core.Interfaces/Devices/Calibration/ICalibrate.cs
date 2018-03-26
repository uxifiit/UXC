using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Devices.Calibration
{
    public interface ICalibrate
    {
        /// <summary>
        /// Returns an instance of the ICalibrator which is specific to the device to calibrate.
        /// </summary>
        /// <returns></returns>
        ICalibrator GetCalibrator();

        event EventHandler<ICalibrator> CalibratorRequested;

        /// <summary>
        /// Checks whether the given calibrator is valid and can be used for calibration of a device.
        /// </summary>
        /// <param name="calibrator"></param>
        /// <returns></returns>
        bool IsCalibratorValid(ICalibrator calibrator);

        ///// <summary>
        ///// Gets a boolean value indicating whether the device can be calibrated at the moment.
        ///// </summary>
        bool CanCalibrate { get; }

        event EventHandler<bool> CanCalibrateChanged;

        ///// <summary>
        ///// Gets a boolean value indicating whether the device is calibrated for recording.
        ///// </summary>
        //bool IsCalibrated { get; }
    }
}
