using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;

namespace UXC.Devices.EyeTracker.Calibration
{
    // TODO Data, load from file instead
    public static class CalibrationPlansFactory
    {
        public static CalibrationPlan CreateBasicPlan9()
        {
            Point2[] points = new Point2[] {
                new Point2(0.1, 0.1),
                new Point2(0.1, 0.5),
                new Point2(0.1, 0.9),
                new Point2(0.5, 0.1),
                new Point2(0.5, 0.5),
                new Point2(0.5, 0.9),
                new Point2(0.9, 0.1),
                new Point2(0.9, 0.5),
                new Point2(0.9, 0.9) 
            };
            return new CalibrationPlan(points);
        }

        public static CalibrationPlan CreateBasicPlan5()
        {
            Point2[] points = new Point2[] {
                new Point2(0.1, 0.1),
                new Point2(0.5, 0.5),
                new Point2(0.9, 0.1),
                new Point2(0.9, 0.9),
                new Point2(0.1, 0.9)
            };
            return new CalibrationPlan(points);
        }

        public static CalibrationPlan CreateDiamondPlan12()
        {
            Point2[] points = new Point2[] 
            {
                new Point2(0.08, 0.08),
                new Point2(0.92, 0.08),
                new Point2(0.08, 0.92),
                new Point2(0.92, 0.92),

                new Point2(0.08, 0.5 ),
                new Point2(0.5 , 0.08),
                new Point2(0.92, 0.5 ),
                new Point2(0.5 , 0.92),

                new Point2(0.2 , 0.2 ),
                new Point2(0.8 , 0.8 ),
                new Point2(0.8 , 0.2 ),
                new Point2(0.2 , 0.8 )
            };
            return new CalibrationPlan(points);
        }
    }
}


