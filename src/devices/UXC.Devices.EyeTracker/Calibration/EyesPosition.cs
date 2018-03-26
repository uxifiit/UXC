using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;

namespace UXC.Devices.EyeTracker.Calibration
{
    public class EyesPosition
    {
        internal EyesPosition(Point2 leftEyePosition, bool isLeftEyeVisible, Point2 rightEyePosition, bool isRightEyeVisible, double? distance, double? relativeDistance, DistanceRecommendation recommendation)
        {
            LeftEyePosition2D = leftEyePosition;
            RightEyePosition2D = rightEyePosition;
            IsLeftEyeVisible = isLeftEyeVisible;
            IsRightEyeVisible = isRightEyeVisible;
            Distance = distance;
            RelativeDistance = relativeDistance;
            Recommendation = recommendation;
        }

        public Point2 LeftEyePosition2D { get; } 
        public Point2 RightEyePosition2D { get; }

        public bool IsLeftEyeVisible { get; }
        public bool IsRightEyeVisible { get; }

        public double? Distance { get; } 

        public double? RelativeDistance { get; }

        public DistanceRecommendation Recommendation { get; }
    }
}
