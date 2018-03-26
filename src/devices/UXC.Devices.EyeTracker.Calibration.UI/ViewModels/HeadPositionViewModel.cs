using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.ViewModels;
using UXC.Devices.EyeTracker.Calibration;
using UXI.Common.UI;

namespace UXC.Devices.EyeTracker.ViewModels
{
    public class HeadPositioningViewModel : BindableBase, IPointsViewModel
    {
        private readonly IDisposable _positioningSubscription;
        private const double SMOOTHING_ALPHA = 0.3d;

        public HeadPositioningViewModel(HeadPositioning positioning)
        {
            var distanceUpdateInterval = Observable.Interval(TimeSpan.FromSeconds(2));

            _positioningSubscription = Observable.FromEventPattern<EyesPosition>
            (
                h => positioning.PositionChanged += h,
                h => positioning.PositionChanged -= h
            ).Select(e => e.EventArgs)
             .Do(p => UpdatePosition(p))
             .Sample(TimeSpan.FromMilliseconds(100))
             .Do(p => UpdateDistance(p.Distance, p.RelativeDistance))
             .Subscribe();
        }


        private void UpdatePosition(EyesPosition position)
        {
            if (position.IsLeftEyeVisible)
            {
                LeftEye.UpdatePosition(position.LeftEyePosition2D.X, position.LeftEyePosition2D.Y);
                LeftEye.IsVisible = true;
            }
            else
            {
                LeftEye.IsVisible = false;
            }

            if (position.IsRightEyeVisible)
            {
                RightEye.UpdatePosition(position.RightEyePosition2D.X, position.RightEyePosition2D.Y);
                RightEye.IsVisible = true;
            }
            else
            {
                RightEye.IsVisible = false;
            }
        }

        
        public IEnumerator<IPointViewModel> GetEnumerator()
        {
            yield return LeftEye;
            yield return RightEye;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public PointViewModel LeftEye { get; } = new PointViewModel();
        public PointViewModel RightEye { get; } = new PointViewModel();


        private void UpdateDistance(double? absoluteDistance, double? newRelativeDistance)
        {
            if (absoluteDistance.HasValue)
            {
                _rawDistance = ExponentialSmoothing.Smooth(_rawDistance, absoluteDistance.Value, SMOOTHING_ALPHA);
                Distance = Math.Round(_rawDistance / 10);
            }

            if (newRelativeDistance.HasValue)
            {
                RelativeDistance = Math.Min(Math.Max(ExponentialSmoothing.Smooth(RelativeDistance, newRelativeDistance.Value, SMOOTHING_ALPHA), MinRelativeDistance), MaxRelativeDistance);
                ShowRelativeDistance = true;
            }
        }


      

        private double _rawDistance = 0d;
        private double distance = 0d;
        public double Distance { get { return distance; } private set { Set(ref distance, value); } }


        public double MinRelativeDistance => 0d;
        public double MaxRelativeDistance => 1d;


        private double relativeDistance = 0d;
        public double RelativeDistance { get { return relativeDistance; } private set { Set(ref relativeDistance, value); } }


        private bool showRelativeDistance = false;
        public bool ShowRelativeDistance { get { return showRelativeDistance; } private set { Set(ref showRelativeDistance, value); } }


        private DistanceRecommendation recommendation;
        public DistanceRecommendation Recommendation
        {
            get { return recommendation; }
            private set
            {
                if (Set(ref recommendation, value))
                {
                    OnPropertyChanged(nameof(RecommendationString));
                }
            }
        }


        public string RecommendationString
        {
            get
            {
                switch (recommendation)
                {
                    case DistanceRecommendation.TooClose:
                        return "You sit too close.";
                    case DistanceRecommendation.TooFar:
                        return "You sit too far.";
                    case DistanceRecommendation.Unknown:
                        return "Where are you?";
                }
                return "You can start the calibration.";
            }
        }
    }
}
