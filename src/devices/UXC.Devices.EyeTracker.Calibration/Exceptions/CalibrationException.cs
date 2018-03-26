using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Devices.EyeTracker
{
    public class CalibrationException : Exception
    {
        public CalibrationException(string message) : base(message) { }

        public CalibrationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
