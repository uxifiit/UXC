using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Devices.EyeTracker.Calibration
{
    public class CalibrationData
    {
        public CalibrationData(string familyName, string name, byte[] data)
        {
            DeviceFamilyName = familyName;
            DeviceName = name;
            Data = data.ToArray();
        }

        public string DeviceFamilyName { get; }

        public string DeviceName { get; }

        public byte[] Data { get; }
    }
}
