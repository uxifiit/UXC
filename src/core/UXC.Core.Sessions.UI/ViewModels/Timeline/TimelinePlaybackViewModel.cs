using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight.CommandWpf;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Executors;
using UXC.Sessions.Timeline.Results;
using UXC.Sessions.ViewModels.Timeline;
using UXI.Common.Extensions;
using UXI.Common.Helpers;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels
{
    public class TimelinePlaybackViewModel : BindableBase
    {
        private readonly SessionRecordingViewModel _recording;
        private readonly ViewModelResolver _resolver;
        private readonly Dispatcher _dispatcher;

        private readonly SerialDisposable _timerDisposable = new SerialDisposable();
        private readonly SerialDisposable _hotkeysDisposable = new SerialDisposable();

        private SessionStepExecution _execution;

        public TimelinePlaybackViewModel(SessionRecordingViewModel recording, ViewModelResolver resolver, Dispatcher dispatcher)
        {
            _recording = recording;
            _resolver = resolver;
            _dispatcher = dispatcher;
        }


        private IEnumerable<Key> hotkeys = Enumerable.Empty<Key>();
        public IEnumerable<Key> Hotkeys
        {
            get { return hotkeys; }
            private set { Set(ref hotkeys, value); }
        }


        private ITimelineStepViewModel action = null;
        public ITimelineStepViewModel Action
        {
            get { return action; }
            private set
            {
                ITimelineStepViewModel previous;
                if (Set(ref action, value, out previous))
                {
                    if (previous != null)
                    {
                        previous.Completed -= Action_Completed;
                        previous.IsContentChanged -= Action_IsContentChanged;
                    }

                    if (value != null)
                    {
                        value.Completed += Action_Completed;
                        value.IsContentChanged += Action_IsContentChanged;
                    }
                }
            }
        }


        private void Action_Completed(object sender, SessionStepResult result)
        {
            var action = (ITimelineStepViewModel)sender;
            if (Action == action)
            {
                ClearCallbacks();

                _execution.Result = result;

                StepCompleted?.Invoke(this, _execution);
            }
        }


        private void Action_IsContentChanged(object sender, bool isContent)
        {
            _dispatcher.InvokeAsync(() =>
            {
                _recording.IsTimelineActive = isContent;
            }).Task.Forget();
        }


        public void Proceed(SessionStepExecution execution)
        {
            ClearCallbacks();
            _hotkeysDisposable.Disposable = Disposable.Empty;

            _execution = execution;

            // wait for action executor completed
            // -> event Completed
            ITimelineStepViewModel action = ResolveStepActionViewModel(execution.Step.Action);

            // start timer
            // step.Completion.Duration
            // -> event Completed
            if (execution.Step.Completion.Duration.HasValue)
            {
                _timerDisposable.Disposable = new Timer(CompleteActionDurationTimer, action, execution.Step.Completion.Duration.Value, TimeSpan.FromMilliseconds(-1));
            }

            // set key shortcut
            // -> command
            // -> event Completed
            var hotkeys = ResolveHotkeys(execution.Step.Completion.Hotkeys);
            if (hotkeys.Any())
            {
                _hotkeysDisposable.Disposable = RegisterHotkeys(hotkeys, action);
            }

            Action = action;

            action.Execute(_recording);

            _recording.IsTimelineActive = action.IsContent;
        }

      
        private RelayCommand completeCommand;
        public RelayCommand CompleteCommand => completeCommand
            ?? (completeCommand = new RelayCommand(() => Complete(Action)));


        private bool Complete(ITimelineStepViewModel action)
        {
            ClearCallbacks();

            if (action != null && Action == action)
            {
                action.Completed -= Action_Completed;

                _execution.Result = action.Complete();      // Complete may be called twice

                StepCompleted?.Invoke(this, _execution);

                return true;
            }

            return false;
        }
        

        private void CompleteActionDurationTimer(object state)
        {
            _dispatcher.Invoke(() => Complete((ITimelineStepViewModel)state));
        }


        public void Clear()
        {
            ClearCallbacks();

            var lastAction = Action;
            Action = null;

            lastAction?.Complete();
        }


        private void ClearCallbacks()
        {
            _timerDisposable.Disposable = Disposable.Empty;
        }


        private ITimelineStepViewModel ResolveStepActionViewModel(SessionStepActionSettings action)
        {
            ITimelineStepViewModel stepViewModel = null;
            if (_resolver.CanCreate(action))
            {
                stepViewModel = (ITimelineStepViewModel)_resolver.Create(action);
            }

            return stepViewModel;
        }


        private ISet<Key> ResolveHotkeys(IEnumerable<Hotkey> hotkeys)
        {
            var hotkeysType = typeof(Hotkey);
            IEnumerable<string> keyNames = hotkeys?.Where(hotkey => Enum.IsDefined(hotkeysType, hotkey))
                                                   .Select(hotkey => Enum.GetName(hotkeysType, hotkey));

            HashSet<Key> keys = new HashSet<Key>();
            Key key; 
            foreach (var keyName in keyNames)
            {
                if (Enum.TryParse<Key>(keyName, false, out key))
                {
                    keys.Add(key);
                }
            }

            return keys;
        }


        private IDisposable RegisterHotkeys(IEnumerable<Key> hotkeys, ITimelineStepViewModel action)
        {
            List<IDisposable> registrations = new List<IDisposable>();

            foreach (var hotkey in hotkeys)
            {
                string name = $"timeline_{hotkey.ToString()}";

                try
                {
                    NHotkey.Wpf.HotkeyManager.Current.AddOrReplace(name, hotkey, System.Windows.Input.ModifierKeys.None, (_, __) => Complete(action));

                    registrations.Add(Disposable.Create(() =>
                    {
                        NHotkey.Wpf.HotkeyManager.Current.Remove(name);
                    }));
                }
                catch (NHotkey.HotkeyAlreadyRegisteredException ex)
                {
                    // TODO LOG ! or show error !!!
                }
            }

            return registrations.Any()
                 ? new CompositeDisposable(registrations)
                 : Disposable.Empty;
        }


        public event EventHandler<SessionStepExecution> StepCompleted;
    }
}
