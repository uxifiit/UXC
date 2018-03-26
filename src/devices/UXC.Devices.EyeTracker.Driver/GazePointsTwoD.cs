using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;
using UXC.Models;

namespace UXC.Devices.EyeTracker.Utils
{
    public class GazePointsTwoD
    {
        public GazePointsTwoD(double leftX, double leftY, double rightX, double rightY)
        {
            Left = new PointTwoD(leftX, leftY);
            Right = new PointTwoD(rightX, rightY);
        }
        public PointTwoD Left { get; }
        public PointTwoD Right { get; }

        public GazePointsTwoD()
        {
            Left = new PointTwoD();
            Right = new PointTwoD();
        }
        public GazePointsTwoD(PointTwoD left, PointTwoD right)
        {
            Left = left;
            Right = right;
        }
    }
}
