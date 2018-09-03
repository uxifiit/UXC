using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UXC.Core.Common.Commands;
using UXC.Core.Devices;
using UXC.Core.Devices.Calibration;
using UXC.Core.Managers;
using UXC.Core.Modules;
using UXC.Core.ViewModels;
using UXC.Core.ViewModels.Adapters;
using UXC.Core.ViewServices;
using UXC.Devices.Adapters;
using UXC.Devices.EyeTracker.Calibration;
using UXC.Devices.EyeTracker.ViewModels;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;
using UXI.Common.Extensions;
using UXI.Common.Helpers;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels.Timeline
{
    public class EyeTrackerCalibrationTimelineStepViewModel : ContentTimelineStepViewModelBase
    {
        private readonly EyeTrackerCalibrationActionSettings _settings;
        private readonly ViewModelResolver _resolver;
        private readonly ICalibrate _calibrate;

        private readonly List<CalibrationResult> _calibrations = new List<CalibrationResult>();

        private IDisposable _cancellation = null;

        public EyeTrackerCalibrationTimelineStepViewModel(EyeTrackerCalibrationActionSettings settings, IAdaptersManager adapters, ViewModelResolver resolver)
            : base(settings)
        {
            _settings = settings;
            _resolver = resolver;

            var eyeTracker = adapters.FirstOrDefault(a => a.Code.DeviceType == DeviceType.Physiological.EYETRACKER);

            if (eyeTracker != null)
            {
                _calibrate = eyeTracker?.Calibration;

                var adapter = (AdapterViewModel)resolver.Create(eyeTracker);

                ConnectCommand = new WrappedCommand(adapter.ExecuteActionCommand, DeviceAction.Connect);
            }

            Cursor = Cursors.Arrow;
        }


        public INavigationService Navigation { get; } = new ContentPresenterNavigationService();



        public override SessionStepResult Complete()
        {
            _cancellation?.Dispose();
            _cancellation = null;

            var result = SessionStepResult.Failed;

            OnCompleted(result);

            return result;
        }


        public override void Execute(SessionRecordingViewModel recording)
        {
            if (_calibrate != null
                && recording.Recording.SelectedDevices.Any(d => d.Equals(DeviceType.Physiological.EYETRACKER)))
            {
                TryStartCalibrationAsync(_calibrate).Forget();
            }
            else
            {
                OnCompleted(SessionStepResult.Skipped);
            }
        }


        public ICommand ConnectCommand { get; } = NullCommand.Instance;


        public bool CanCalibrate => _calibrate != null && _calibrate.CanCalibrate;


        private async Task TryStartCalibrationAsync(ICalibrate calibrate)
        {
            if (CanCalibrate == false)
            {
                using (var cancellation = new CancellationDisposable())
                {
                    _cancellation = cancellation;

                    await AsyncHelper.InvokeAsync<EventHandler<bool>, bool>
                    (
                        () => { },
                        h => calibrate.CanCalibrateChanged += h,
                        h => calibrate.CanCalibrateChanged -= h,
                        tcs => (_, canCalibrate) =>
                        {
                            if (canCalibrate)
                            {
                                tcs.TrySetResult(canCalibrate);
                            }
                        },
                        cancellation.Token
                    );

                    OnPropertyChanged(nameof(CanCalibrate));

                    _cancellation = null;
                }
            }

            if (CanCalibrate)
            {
                var calibrator = calibrate.GetCalibrator();

                var calibratorViewModel = (CalibratorViewModel)_resolver.Create(calibrator);

                ApplySettings(calibratorViewModel);
              
                Navigation.NavigateToObject(calibratorViewModel);

                await CalibrateAsync(calibrator);
            }
            else
            {
                Complete();
            }
        }


        private void ApplySettings(CalibratorViewModel calibratorViewModel)
        {
            // reset plan to custom if specified in the step settings
            if (_settings.CustomPlan != null && _settings.CustomPlan.Any())
            {
                var plan = new CalibrationPlan(_settings.CustomPlan);
                calibratorViewModel.PlansSelection.Items.Insert(0, plan);
                calibratorViewModel.PlansSelection.SelectedIndex = 0;
            }

            // enable or disable load/save options for calibration profiles
            calibratorViewModel.IsProfileStorageEnabled = _settings.IsProfileEnabled;
        }

        private async Task CalibrateAsync(ICalibrator calibrator)
        {
            var execution = calibrator as IEyeTrackerCalibrationExecution;

            var result = SessionStepResult.Failed;

            using (var cancellation = new CancellationDisposable())
            {
                _cancellation = cancellation;

                try
                {
                    execution.CalibrationFinished += Execution_CalibrationFinished;

                    await AsyncHelper.InvokeAsync<CalibratorStateChangedEventHandler, bool>
                    (
                        () => { },
                        h => calibrator.StateChanged += h,
                        h => calibrator.StateChanged -= h,
                        tcs => (c, state) =>
                        {
                            switch (state)
                            {
                                case CalibratorState.Canceled:
                                    tcs.TrySetResult(false);
                                    break;
                                case CalibratorState.Completed:
                                    tcs.TrySetResult(true);
                                    break;
                            }
                        },
                        cancellation.Token
                    );

                    // create result with calibrations
                    result = new EyeTrackerCalibrationSessionStepResult(_calibrations);
                }
                catch (OperationCanceledException)
                {
                    await calibrator.CancelAsync();
                }
                finally
                {
                    //remove handler from execution
                    if (execution != null)
                    {
                        execution.CalibrationFinished -= Execution_CalibrationFinished;
                    }
                }

                Navigation.Clear();

                _cancellation = null;
            }

            OnCompleted(result);
        }


        private void Execution_CalibrationFinished(object sender, CalibrationResult e)
        {
            if (e != null)
            {
                _calibrations.Add(e);
            }
        }
    }
}
