using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;
using UXI.Common.Extensions;
using UXI.Common.UI;
using System.Windows.Media;

namespace UXC.Sessions.ViewModels.Timeline
{
    public class EyeTrackerValidationTimelineStepViewModel : ContentTimelineStepViewModelBase
    {
        private readonly EyeTrackerValidationActionSettings _settings;
        private readonly List<PointDisplayTime> _displayedPoints = new List<PointDisplayTime>();
        private int _sequenceLength = 0;


        private static readonly List<Point2> DefaultPlan = new List<Point2>()
        {
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


        public EyeTrackerValidationTimelineStepViewModel(EyeTrackerValidationActionSettings settings)
            : base(settings)
        {
            _settings = settings;

            PointFill = ResolveColor(settings.PointColor, Colors.LimeGreen);

            Message = settings.Message?.Trim();
        }


        public TimeSpan? InstructionsDuration => _settings.InstructionsDuration;


        public override void Execute(SessionRecordingViewModel recording)
        {
            if (recording.Recording.SelectedDevices.Any(d => d.Equals(DeviceType.Physiological.EYETRACKER)))
            {
                var points = _settings.Points?.ToList();

                if (points == null || points.Any() == false)
                {
                    points = DefaultPlan;
                }

                if (points.Any())
                {
                    _sequenceLength = points.Count;

                    SetupAnimation(points);
                }
                else
                {
                    Complete();
                }
            }
            else
            {
                OnCompleted(SessionStepResult.Skipped);
            }
        }


        public Brush PointFill { get; }


        public string Message { get; }


        private void SetupAnimation(IEnumerable<Point2> points)
        {
            var sequence = new PointsSequence(points?.Select(p => new Point(p.X, p.Y)));

            var animation = new PointAnimationViewModel(sequence);
            animation.Completed += PointAnimation_Completed;
            animation.Cancelled += PointAnimation_Cancelled;
            animation.PointCompleted += PointAnimation_PointCompleted;

            Animation = animation;
        }


        private async void PointAnimation_PointCompleted(object sender, Point point)
        {
            var startTime = DateTime.Now;

            if (_settings.PointDuration.HasValue)
            {
                await Task.Delay(_settings.PointDuration.Value);
            }

            var endTime = DateTime.Now;

            _displayedPoints.Add(new PointDisplayTime(new Point2(point.X, point.Y), startTime, endTime));

            Animation.Continue();
        }


        private void PointAnimation_Cancelled(object sender, EventArgs e)
        {
            Complete();
        }


        private void PointAnimation_Completed(object sender, EventArgs e)
        {
            Complete();
        }


        private PointAnimationViewModel animation;
        public PointAnimationViewModel Animation
        {
            get { return animation; }
            private set { Set(ref animation, value); }
        }


        private void SaveValidationToRecordingSettings()
        {
         //   _recording.Settings.SetCustomSetting("", points, )
        }


        public override SessionStepResult Complete()
        {
            var result = SessionStepResult.Skipped;

            if (_displayedPoints.Any())
            {
                bool isCompleted = _displayedPoints.Count == _sequenceLength;

                result = new EyeTrackerValidationSessionStepResult(
                    isCompleted ? SessionStepResultType.Successful : SessionStepResultType.Skipped, 
                    _displayedPoints
                );

                SaveValidationToRecordingSettings();
            }

            OnCompleted(result);

            return result;
        }
    }
}
