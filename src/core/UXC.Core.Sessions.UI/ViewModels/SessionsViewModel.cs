using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight.CommandWpf;
using UXC.Core.Common.Commands;
using UXC.Core.Common.Events;
using UXC.Core.Devices;
using UXC.Core.ViewServices;
using UXI.Common.Extensions;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels
{
    public class SessionsViewModel : BindableBase
    {
        private readonly Dispatcher _dispatcher;
        private readonly ISessionsControl _control;
        private readonly IViewsService _views;
        private readonly SessionRecordingViewModelFactory _factory;

        private IView _sessionHostWindow = null;

        public SessionsViewModel(ISessionsControl control, IViewsService views, SessionRecordingViewModelFactory factory, SessionDefinitionsViewModel definitions, Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            _control = control;
            _views = views;

            _factory = factory;

            if (_control.CurrentRecording != null)
            {
                UpdateRecording(_control.CurrentRecording);
            }
            _control.RecordingChanged += (_, recording) => dispatcher.Invoke(() => UpdateRecording(recording));

            Definitions = definitions;
        }


        public SessionDefinitionsViewModel Definitions { get; }


        private void UpdateRecording(SessionRecording recording)
        {
            CloseHostWindow();

            if (recording != null)
            {
                CurrentRecording = (SessionRecordingViewModel)_factory.Create(recording);

                var hostWindow = _views.SessionHostWindow;
                hostWindow.Closed += SessionHostWindow_Closed;
                //CurrentRecording.HostWindow = hostWindow;

                //    hostWindow.Show();
                // minimize main window ? or through behavior in hostwindow
                _sessionHostWindow = hostWindow;                        // do we need this, use CurrentRecording.HostWindow for control
            }
            else
            {
                CurrentRecording = null;

                if (_views.MainWindow != null && _views.MainWindow.IsClosed == false)
                {
                    _views.MainWindow.MakeVisible();
                }
            }
        }


        private SessionRecordingViewModel currentRecording = null;
        public SessionRecordingViewModel CurrentRecording
        {
            get { return currentRecording; }
            private set
            {
                SessionRecordingViewModel previous;
                if (Set(ref currentRecording, value, out previous))
                {
                    OnPropertyChanged(nameof(HasSession));
                    OnPropertyChanged(nameof(HasActiveSession));

                    showSessionCommand?.RaiseCanExecuteChanged();

                    if (previous != null)
                    {
                        previous.RecordingEnded -= SessionRecording_RecordingEnded;
                        previous.TimelineActiveChanged -= SessionRecording_TimelineActiveChanged;
                        previous.StateChanged -= SessionRecording_StateChanged;
                    }

                    if (value != null)
                    {
                        value.RecordingEnded += SessionRecording_RecordingEnded;
                        value.TimelineActiveChanged += SessionRecording_TimelineActiveChanged;
                        value.StateChanged += SessionRecording_StateChanged;
                    }
                }
            }
        }


        private void SessionRecording_StateChanged(object sender, SessionState state)
        {
            if (state == SessionState.Preparing)
            {
                _views.MainWindow?.Hide();
            }

            OnPropertyChanged(nameof(HasActiveSession));
        }


        private void SessionRecording_TimelineActiveChanged(object sender, bool isTimelineActive)
        {
            showSessionCommand?.RaiseCanExecuteChanged();
            var hostWindow = _sessionHostWindow;
            if (hostWindow != null)
            {
                if (isTimelineActive)
                {
                    ShowSession(hostWindow);

                    hostWindow.Navigation?.NavigateToObject(CurrentRecording);
                }
                else
                {
                    hostWindow.Hide();
                }
            }
        }

        private void SessionRecording_RecordingEnded(object sender, EventArgs e)
        {
            CloseHostWindow();
            _views.MainWindow?.MakeVisible();
        }

        public bool HasSession => currentRecording != null;

        public bool HasActiveSession => currentRecording != null && currentRecording.IsActive;


        private void CloseHostWindow()
        {
            var hostWindow = ObjectEx.GetAndReplace(ref _sessionHostWindow, null);
            if (hostWindow != null)
            {
                hostWindow.Closed -= SessionHostWindow_Closed;
                hostWindow.Close();
            }
        }


        private void SessionHostWindow_Closed(object sender, EventArgs args)
        {
            _control.Close();
            _views.MainWindow?.MakeVisible();

            Definitions.RefreshAsync().Forget();
            // REFRESH
        }


        private RelayCommand<ISessionChoiceViewModel> openCommand = null;
        public RelayCommand<ISessionChoiceViewModel> OpenCommand => openCommand
            ?? (openCommand = new RelayCommand<ISessionChoiceViewModel>(d => OpenSessionAsync(d).Forget(), d => d != null));

        private async Task OpenSessionAsync(ISessionChoiceViewModel definition)
        {
            try
            {
                SessionRecording recording = _control.Record(definition.GetDefinition());

                if (currentRecording != null && currentRecording.Recording == recording)
                {
                    bool open = await currentRecording.OpenAsync();

                    if (open)
                    {
                        _views.MainWindow?.Hide();
                    }
                }
            }
            catch
            {
                // TODO LOG
            }
        }


        private RelayCommand clearCommand = null;
        public RelayCommand ClearCommand => clearCommand
            ?? (clearCommand = new RelayCommand(_control.Clear));


        private RelayCommand closeCommand = null;
        public RelayCommand CloseCommand => closeCommand
            ?? (closeCommand = new RelayCommand(_control.Close)); // depends on HasRunningSession


        private RelayCommand showSessionCommand = null; // opens session window from the app main window
        public RelayCommand ShowSessionCommand => showSessionCommand
            ?? (showSessionCommand = new RelayCommand(() => ShowSession(_sessionHostWindow), () => CurrentRecording != null && CurrentRecording.IsTimelineActive == true));

        private void ShowSession(IView hostWindow)
        {
            if (hostWindow != null)
            {
                _views.MainWindow?.Hide();

                if (hostWindow.IsClosed)
                {
                    hostWindow.Show();
                }
                else
                {
                    hostWindow.MakeVisible();
                }
                hostWindow.Activate();
            }
        }
    }
}
