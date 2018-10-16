using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.UI;
using UXC.Core.Data;
using UXC.Devices.EyeTracker.Calibration;
using GalaSoft.MvvmLight.CommandWpf;
using UXI.Common.Extensions;
using System.Windows;
using UXC.Core.ViewModels;
using System.Windows.Input;

namespace UXC.Devices.EyeTracker.ViewModels
{
    public class CalibrationViewModel : BindableBase
    {
        private readonly EyeTrackerCalibrator _calibrator;

        public CalibrationViewModel(EyeTrackerCalibrator calibrator)
        {
            _calibrator = calibrator;
        }

        public void ResetPlan(CalibrationPlan plan)
        {
            Animation?.Cancel();

            var sequence = new PointsSequence(plan?.Select(p => new Point(p.X, p.Y)));

            Animation = new PointAnimationViewModel(sequence);
        }


        private PointAnimationViewModel animation;
        public PointAnimationViewModel Animation
        {
            get { return animation; }
            private set
            {
                value.ThrowIfNull(nameof(value));

                PointAnimationViewModel previous;
                if (Set(ref animation, value, out previous))
                {
                    value.Cancelled += PointAnimation_Cancelled;
                    value.Completed += PointAnimation_Completed;
                    value.PointCompleted += PointAnimation_PointCompleted;

                    if (previous != null)
                    {
                        previous.Cancelled -= PointAnimation_Cancelled;
                        previous.Completed -= PointAnimation_Completed;
                        previous.PointCompleted -= PointAnimation_PointCompleted;
                    }
                }
            }
        }


        private async void PointAnimation_Cancelled(object sender, EventArgs e)
        {
            try
            {
                await _calibrator.CancelAsync();
            }
            catch (Exception ex)
            {
                // TODO ex
            }

            Cancelled?.Invoke(this, EventArgs.Empty);
        }


        private async void PointAnimation_Completed(object sender, EventArgs e)
        {
            try
            {
                if (_calibrator.CanFinish())
                {
                    var result = await _calibrator.FinishAsync();
                    Completed?.Invoke(this, result);
                }
                else
                {
                    // TODO log
                }
            }
            catch (Exception ex)
            {
                Failed?.Invoke(this, ex);
                 // TODO ex
            }
        }


        public TimeSpan PointCompletionBeginTimeout { get; set; } = TimeSpan.FromMilliseconds(1000);

        public TimeSpan PointCompletionEndTimeout { get; set; } = TimeSpan.FromMilliseconds(300);


        private async void PointAnimation_PointCompleted(object sender, Point point)
        {
            try
            {
                Point2 point2d = new Point2(point.X, point.Y);
                if (_calibrator.CanContinue(point2d))
                {
                    await Task.Delay(PointCompletionBeginTimeout); // PointCompletedBeginTimeout

                    await _calibrator.ContinueAsync(point2d);

                    await Task.Delay(PointCompletionEndTimeout); // PointCompletedEndTimeout

                    Animation?.Continue();
                }
            }
            catch (Exception ex)
            {
                Animation?.Stop();
                Failed?.Invoke(this, ex);
            }
        }


        public void Start()
        {
            Animation?.Start();
        }


        private ICommand cancelCommand;
        public ICommand CancelCommand => cancelCommand
            ?? (cancelCommand = new RelayCommand(Cancel));

        public void Cancel()
        {
            Animation?.Cancel();
        }


        public event EventHandler<CalibrationExecutionReport> Completed;
        public event EventHandler Cancelled;
        public event EventHandler<Exception> Failed;
    }
}
