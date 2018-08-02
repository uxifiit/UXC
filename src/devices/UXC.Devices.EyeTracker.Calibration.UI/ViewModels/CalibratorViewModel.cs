using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;
using UXI.Common.UI;
using UXC.Core.Data;
using UXC.Core.ViewServices;
using UXC.Devices.EyeTracker.Calibration;
using System.Windows.Input;
using UXC.Core.Common.Extensions;
using UXC.Devices.EyeTracker.Models;
using System.Collections.ObjectModel;
using System.Windows;
using UXC.Core.ViewModels;

namespace UXC.Devices.EyeTracker.ViewModels
{
    public class CalibratorViewModel : BindableBase, INavigatingViewModel
    {
        private readonly EyeTrackerCalibrator _calibrator;

        private readonly List<ICommand> _commands = new List<ICommand>();

        public CalibratorViewModel(EyeTrackerCalibrator calibrator, ICalibrationProfilesService calibrations)
        {
            calibrator.ThrowIfNull(nameof(calibrator));

            _calibrator = calibrator;
            _calibrator.StateChanged += (_, __) => _commands.TryRaiseCanExecuteChanged();

            var plans = new List<CalibrationPlan>()
            {
                CalibrationPlansFactory.CreateBasicPlan9(),
                CalibrationPlansFactory.CreateBasicPlan5(),
                CalibrationPlansFactory.CreateDiamondPlan12(),
            };
            PlansSelection = new SelectionViewModel<CalibrationPlan>(plans)
            {
                SelectedIndex = 0
            };

            Calibration = new CalibrationViewModel(_calibrator);
            Calibration.Completed += Calibration_Completed;
            Calibration.Cancelled += Calibration_Cancelled;

            HeadPosition = new HeadPositioningViewModel(_calibrator.HeadPositioning);
            StoredCalibrations = new StoredCalibrationsViewModel(calibrations);
            StoredCalibrations.CalibrationLoaded += StoredCalibrations_CalibrationLoaded;
        }

        private void Calibration_Completed(object sender, CalibrationExecutionReport result)
        {
            UpdateResult(result);
        }


        private void Calibration_Cancelled(object sender, EventArgs e)
        {
            NavigationService?.GoHome();
        }

        private async void StoredCalibrations_CalibrationLoaded(object sender, CalibrationData data)
        {
            if (data != null && _calibrator.CanRestore(data))
            {
                await _calibrator.RestoreAsync(data);
            }
            else
            {
                // TODO show error
            }
        }


        public EyeTrackerDeviceInfo DeviceInfo => _calibrator.DeviceInfo;


        public HeadPositioningViewModel HeadPosition { get; }


        public StoredCalibrationsViewModel StoredCalibrations { get; }


        private RelayCommand prepareCommand = null;
        public RelayCommand PrepareCommand => prepareCommand
            ?? (prepareCommand = _commands.Register(() => 
            {
                _calibrator.Prepare();

                StoredCalibrations.Refresh(_calibrator.DeviceInfo);
            }, _calibrator.CanPrepare));



        public SelectionViewModel<CalibrationPlan> PlansSelection { get; }
        
        //public IEnumerable<CalibrationPlan> Plans { get; }

        //private CalibrationPlan selectedPlan;
        //public CalibrationPlan SelectedPlan
        //{
        //    get { return selectedPlan; }
        //    set { Set(ref selectedPlan, value); }
        //}


        private RelayCommand<CalibrationPlan> startCommand = null;
        public RelayCommand<CalibrationPlan> StartCommand => startCommand
            ?? (startCommand = _commands.Register<CalibrationPlan>(plan => StartAsync(plan).Forget(), _calibrator.CanStart));

        private async Task<bool> StartAsync(CalibrationPlan plan)
        {
            Calibration.ResetPlan(plan);

            await _calibrator.StartAsync(plan);

            NavigationService.NavigateToObject(Calibration);

            return true;
        }


        private RelayCommand submitCommand = null;
        public RelayCommand SubmitCommand => submitCommand
            ?? (submitCommand = _commands.Register(() =>
                {
                    //_dialog.Close();
                    _calibrator.Submit();
                }, _calibrator.CanSubmit));


        private RelayCommand cancelCommand = null;
        public RelayCommand CancelCommand => cancelCommand
            ?? (cancelCommand = _commands.Register(async () =>
                {
                    //_dialog.Close();
                    await _calibrator.CancelAsync();
                }));


        private RelayCommand retryCommand = null;
        public RelayCommand RetryCommand => retryCommand
            ?? (retryCommand = _commands.Register(() =>
            {
                _calibrator.Retry();
                NavigationService?.GoHome();
            }, _calibrator.CanRetry));


        public CalibrationViewModel Calibration { get; }


        private bool isProfileStorageEnabled = false;
        public bool IsProfileStorageEnabled
        {
            get { return isProfileStorageEnabled; }
            set
            {
                if (Set(ref isProfileStorageEnabled, value))
                {
                    OnPropertyChanged(nameof(ProfileStorageVisibility));
                }
            }
        }

        public Visibility ProfileStorageVisibility => IsProfileStorageEnabled ? Visibility.Visible : Visibility.Collapsed;


        private void UpdateResult(CalibrationExecutionReport result)
        {
            Result = new CalibrationResultViewModel(this, result.Plan, result.Result);

            if (IsProfileStorageEnabled && Result.IsValid)
            {
                StoredCalibrations.PrepareSave(result.Data);
                CanSaveCalibration = true;
            }
            else
            {
                CanSaveCalibration = false;
            }

            NavigationService?.NavigateToObject(Result);
        }


        private CalibrationResultViewModel result;
        public CalibrationResultViewModel Result { get { return result; } private set { Set(ref result, value); } }


        private bool canSaveCalibration = false;
        public bool CanSaveCalibration
        {
            get { return canSaveCalibration; }
            private set
            {
                if (Set(ref canSaveCalibration, value))
                {
                    OnPropertyChanged(nameof(SaveCalibrationVisibility));
                }
            }
        }

        public Visibility SaveCalibrationVisibility => CanSaveCalibration ? Visibility.Visible : Visibility.Collapsed;


        public INavigationService NavigationService { get; set; }

        public event EventHandler Closed;
    }
}
