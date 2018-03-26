using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Devices.EyeTracker
{
    internal static class ExponentialSmoothing
    {
        public static double Smooth(double previous, double current, double alpha)
        {
            return previous * (1 - alpha) + current * alpha;
        }
    }
}
