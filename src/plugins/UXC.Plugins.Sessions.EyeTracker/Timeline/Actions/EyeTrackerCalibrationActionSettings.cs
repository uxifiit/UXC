using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;

namespace UXC.Sessions.Timeline.Actions
{
    public class EyeTrackerCalibrationActionSettings : ContentActionSettinsBase
    {
        //    public int PlanLength { get; set; }
        public List<Point2> CustomPlan { get; set; }

        // Calibration requires mouse cursor, so we disable this option
        public override bool? ShowCursor { get { return null; } set { } }

        //public TimeSpan? Delay { get; set; }

        //public TimeSpan? MovementDuration { get; set; }

        //public TimeSpan? InstructionsDuration { get; set; }

        public override SessionStepActionSettings Clone()
        {
            var clone = (EyeTrackerCalibrationActionSettings)base.Clone();

            clone.CustomPlan = CustomPlan?.ToList();

            return clone;
        }
    }
}
