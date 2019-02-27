using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using UXC.Core.ViewModels;
using UXC.Core.ViewModels.Adapters;
using UXC.Core.ViewServices;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Results;
using UXI.Common.Extensions;
using UXI.Common.UI;
using UXC.Sessions.Helpers;
using UXC.Sessions.ViewModels.Timeline.Preparation;

namespace UXC.Sessions.ViewModels
{
    public class SessionRecordingViewModel : ProgressViewModel
    {
        private readonly Dispatcher _dispatcher;
        private readonly TimelinePreparation _preparation;
        private readonly WindowsTaskbarHelper _taskbar = new WindowsTaskbarHelper();

        public SessionRecordingViewModel(SessionRecording recording, ViewModelResolver resolver, Dispatcher dispatcher, TimelinePreparation preparation)
        {
            _dispatcher = dispatcher;
            _preparation = preparation;
            Recording = recording;
            Recording.StateChanged += Recording_StateChanged;
            Recording.CurrentStepChanged += Recording_CurrentStepChanged;

            Timeline = new TimelinePlaybackViewModel(this, resolver, dispatcher);
            Timeline.StepCompleted += Timeline_StepCompleted;

            if (recording.CurrentStep != null)
            {
                Timeline.Proceed(recording.CurrentStep);
            }
        }

        private void Recording_StartFailed(object sender, EventArgs e)
        {
            _dispatcher.InvokeAsync(() =>
            {
                ShowDevicesNotReady();
            }).Task.Forget();
        }

        private void Timeline_StepCompleted(object sender, SessionStepExecution execution)
        {
            _dispatcher.InvokeAsync(() =>
            {
                if (CanContinueRecording(execution.Result))
                {
                    Recording.Continue();
                }
                else
                {
                    Recording.TryCancel();
                }
            }).Task.Forget();
        }


        private static bool CanContinueRecording(SessionStepResult result)
        {
            return result != null && result.ResultType != SessionStepResultType.Failed;
        }


        private void Recording_CurrentStepChanged(object sender, Core.Common.Events.ValueChangedEventArgs<SessionStepExecution> e)
        {
            if (e.CurrentValue != null)
            {
                _dispatcher.InvokeAsync(() =>
                {
                    continueCommand?.RaiseCanExecuteChanged();

                    Timeline.Proceed(e.CurrentValue);
                }).Task.Forget(ex => 
                {
                    // TODO log exception
                });
            }
        }


        private void Recording_StateChanged(object sender, Core.Common.Events.ValueChangedEventArgs<SessionState> e)
        {
            _dispatcher.InvokeAsync(() =>
            {
                if (Recording.IsRunning && _taskbar.IsChanged == false)
                {
                    Task.Run(() => _taskbar.Hide()).Forget();
                }

                OnPropertyChanged(nameof(State));
                OnPropertyChanged(nameof(IsActive));
                OnPropertyChanged(nameof(IsRunning));

                //startCommand?.RaiseCanExecuteChanged();
                closeCommand?.RaiseCanExecuteChanged();
                continueCommand?.RaiseCanExecuteChanged();

                StateChanged?.Invoke(this, State);

                if (Recording.IsFinished)
                {
                    Task.Run(() => _taskbar.Reset()).Forget();

                    _preparation.Reset();

                    IsTimelineActive = false;
                    Timeline.Clear();
                    RecordingEnded?.Invoke(this, EventArgs.Empty);
                }
            }).Task.Forget();
        }

        public SessionRecording Recording { get; }

        public TimelinePlaybackViewModel Timeline { get; }

        public SessionState State => Recording.State;

        public string Project => Recording.Definition.Project;

        public string Name => Recording.Definition.Name;

        public event EventHandler<SessionState> StateChanged;


        public bool IsActive => Recording.IsActive;


        public bool IsRunning => Recording.IsRunning;


        public event EventHandler RecordingEnded;


        private bool isTimelineActive = false;
        public bool IsTimelineActive
        {
            get { return isTimelineActive; }
            set
            {
                if (Set(ref isTimelineActive, value))
                {
                    TimelineActiveChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<bool> TimelineActiveChanged;


       
        public async Task<bool> OpenAsync()
        {
            try
            {
                IsLoading = true;

                await _preparation.PrepareAsync(Recording);

                IsLoaded = await Recording.OpenAsync(CancellationToken.None);

                // TODO add error if IsLoaded == false
            }
            catch
            {
                throw;
            }
            finally 
            { 
                IsLoading = false;
            }

            return IsLoaded;
        }


        private RelayCommand closeCommand = null;
        public RelayCommand CloseCommand => closeCommand
            ?? (closeCommand = new RelayCommand(() => CloseAsync().Forget(), () => Recording.CanCancel()));

        private async Task CloseAsync()
        {
            if (IsRunning == false 
                || await RequestCloseAsync())
            {
                Timeline.Clear();
                Recording.TryCancel();
            }
        }

        private async Task<bool> RequestCloseAsync()
        {
            var result = await DialogCoordinator.Instance.ShowMessageAsync
            (
                this, 
                "Close session?", 
                "Closing the session will end the recording.",
                MessageDialogStyle.AffirmativeAndNegative,
                new MetroDialogSettings()
                {
                    AffirmativeButtonText = "OK",
                    NegativeButtonText = "Cancel",
                    AnimateShow = false,
                    AnimateHide = false,
                    DialogTitleFontSize = 28,
                    DialogMessageFontSize = 20
                }
            );

            return result == MessageDialogResult.Affirmative;
        }
      

        private RelayCommand continueCommand = null;
        public RelayCommand ContinueCommand => continueCommand
            ?? (continueCommand = new RelayCommand(() => TryContinue()));

        private bool TryContinue()
        {
            return Recording.Continue();

            //if (Recording.CanContinue())
            //{
            //    Recording.Continue();
            //    return true;
            //}
            //else if (Recording.State == SessionState.Open)
            //{
            //    if (Recording.CanStart())
            //    {
            //        Recording.Start();
            //        return true;
            //    }
            //    else
            //    {
            //        ShowDevicesNotReady();
            //        return false;
            //        // show message about devices
            //    }
            //}

            //return false;
            //else if (Recording.CanClose())
            //{
            //    Recording.Close();
            //}
        }


        private void ShowDevicesNotReady()
        {
            //if (IsTimelineActive)
            //{
            //    Timeline.Proceed();
            //}
            //else
            //{
                DialogCoordinator.Instance.ShowMessageAsync
                (
                    this,
                    "Problem occurred",
                    "Recording devices are not prepared for session recording. Try to connect devices manually and try again.",
                    MessageDialogStyle.Affirmative,
                    new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "OK",
                        AnimateShow = false,
                        AnimateHide = false,
                        DialogTitleFontSize = 28,
                        DialogMessageFontSize = 20
                    }
                ).Forget();
            //}
        }
    }
}
