using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;

namespace UXC.Sessions.Timeline.Actions
{

    public class EyeTrackerValidationActionSettings : ContentActionSettingsBase
    {
        public List<Point2> Points { get; set; }

        public int? PointDuration { get; set; } = 2000;

        public double? MovementDuration { get; set; }

        public TimeSpan? MessageDuration { get; set; }

        public string PointColor { get; set; }

        public string Message { get; set; }

        public override SessionStepActionSettings Clone()
        {
            var clone = (EyeTrackerValidationActionSettings)base.Clone();

            clone.Points = Points?.ToList();

            return clone;
        }
    }
}
