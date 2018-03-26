using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;

namespace UXC.Devices.EyeTracker.Driver.Simulator.Extensions
{
    internal static class SimulatorPointsEx
    {
        public static IEnumerable<double> AsEnumerable(this Point2 point)
        {
            yield return point.X;
            yield return point.Y;
        }

        public static Point2 ToPoint2(double[] coordinates)
        {
            return coordinates.Length == 2 
                ? new Point2(coordinates[0], coordinates[1])
                : new Point2();
        }
    }
}
