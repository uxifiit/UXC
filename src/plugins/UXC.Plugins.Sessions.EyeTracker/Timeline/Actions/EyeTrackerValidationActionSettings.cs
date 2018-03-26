using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;

namespace UXC.Sessions.Timeline.Actions
{

    public class EyeTrackerValidationActionSettings : ContentActionSettinsBase
    {
        public List<Point2> Points { get; set; }

        public TimeSpan? PointDuration { get; set; }

        public TimeSpan? MovementDuration { get; set; }

        public TimeSpan? InstructionsDuration { get; set; }

        public string PointColor { get; set; }

        public override SessionStepActionSettings Clone()
        {
            var clone = (EyeTrackerValidationActionSettings)base.Clone();

            clone.Points = Points?.ToList();

            return clone;
        }
    }
}
