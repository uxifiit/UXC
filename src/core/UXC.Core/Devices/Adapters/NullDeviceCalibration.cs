using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices.Calibration;

namespace UXC.Devices.Adapters
{
    class NullDeviceCalibration : ICalibrate
    {
        private static readonly Lazy<NullDeviceCalibration> instance = new Lazy<NullDeviceCalibration>();
        public static NullDeviceCalibration Instance => instance.Value;


        public event EventHandler<ICalibrator> CalibratorRequested { add { } remove { } }


        public ICalibrator GetCalibrator()
        {
            throw new NotSupportedException();
        }

      
        public bool IsCalibratorValid(ICalibrator calibrator) => false;


        public bool CanCalibrate => false;


        public event EventHandler<bool> CanCalibrateChanged { add { } remove { } }
    }
}
