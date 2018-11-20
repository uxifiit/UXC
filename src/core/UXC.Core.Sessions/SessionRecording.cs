using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Stateless;
using UXC.Core.Common.Events;
using UXC.Core.Devices;
using UXC.Core.Managers.Adapters;
using UXC.Sessions.Timeline;
using UXC.Sessions.Recording;
using UXI.Common;
using UXI.Common.Extensions;
using UXI.Configuration.Storages;
using UXC.Sessions.Timeline.Results;
using System.Reactive.Disposables;
using System.Threading;

namespace UXC.Sessions
{
    public class SessionRecording : DisposableBase, INotifyStateChanged<SessionState>
    {
        private enum SessionAction
        {
            Open,
            Cancel,
            Continue,
            Complete
        }

        private readonly SessionDefinition _definition;
        private readonly IAdaptersControl _adapters;

        private readonly StateMachine<SessionState, SessionAction> _stateMachine;

        private readonly ReplaySubject<SessionRecordingEvent> _events = new ReplaySubject<SessionRecordingEvent>(1);

        private readonly LinkedList<SessionStep> _steps = new LinkedList<SessionStep>();
        private readonly LinkedList<SessionStep> _preSteps = new LinkedList<SessionStep>();
        private readonly LinkedList<SessionStep> _postSteps = new LinkedList<SessionStep>();

        private readonly SerialDisposable _cancellation = new SerialDisposable();

        private LinkedList<SessionStep> _currentSteps = null;

        private readonly static SessionRecorderDefinition DefaultRecorderDefinition = new SessionRecorderDefinition("Local");


        internal SessionRecording(SessionDefinition definition, IAdaptersControl adapters)
        {
            definition.ThrowIfNull(nameof(definition));

            _definition = definition;
            _adapters = adapters;

            DateTime openedAt = DateTime.Now;
            string sessionId = CreateRecordingId(openedAt, definition.Project, definition.Name);

            Settings = new SessionRecordingSettings(definition, sessionId, openedAt);

            InsertSteps(_preSteps, definition.PreSessionSteps);
            InsertSteps(_postSteps, definition.PostSessionSteps);
            InsertSteps(_steps, definition.SessionSteps);

            if (_steps.Count == 0)
            {
                InsertStep(_steps, SessionStep.Default);
            }

            if (definition.Welcome != null && definition.Welcome.Ignore == false)
            {
                InsertStep(_preSteps, new SessionStep() { Action = definition.Welcome });
            }

            RecorderConfigurations = ValidateRecorders(definition.Recorders).DefaultIfEmpty(DefaultRecorderDefinition).ToDictionary
            (
                d => d.Name,
                d => (IDictionary<string, object>)d.Configuration.ToDictionary(r => r.Key, r => r.Value)
            );

            DeviceConfigurations = ValidateDevices(definition.Devices).ToDictionary
            (
                d => d.Device,
                d => (IDictionary<string, object>)d.Configuration.ToDictionary(c => c.Key, c => c.Value)
            );
            
            _stateMachine = CreateStateMachine();
        }


        private static string CreateRecordingId(DateTime openedAt, string project, string session)
        {
            string name = $"{project?.Trim()} - {session?.Trim()}";
            string timestamp = openedAt.ToString("s");

            string id = timestamp;
            if (name.Length > 3)
            {
                id += $" {name}";
            }

            return id;
        }


        private StateMachine<SessionState, SessionAction> CreateStateMachine()
        {
            var states = new StateMachine<SessionState, SessionAction>(() => State, state => State = state);

            states.Configure(SessionState.None)
                  .Permit(SessionAction.Open, SessionState.Opening)
                  .Permit(SessionAction.Cancel, SessionState.Cancelled);

            states.Configure(SessionState.Opening)
                  .PermitDynamicIf(SessionAction.Complete, () => _preSteps.Any() ? SessionState.Preparing : SessionState.Running, CheckDeviceStates)
                  .Permit(SessionAction.Cancel, SessionState.Cancelled);

            states.Configure(SessionState.Preparing)
                  .InternalTransition(SessionAction.Continue, TakeNextStep)
                  .Permit(SessionAction.Complete, SessionState.Running)
                  .Permit(SessionAction.Cancel, SessionState.Cancelled)
                  .OnEntryFrom(SessionAction.Complete, Prepare)
                  .OnExit(ClearStep);

            states.Configure(SessionState.Running)
                  .InternalTransition(SessionAction.Continue, TakeNextStep)
                  .PermitDynamic(SessionAction.Complete, () => _postSteps.Any() ? SessionState.Processing : SessionState.Completed)
                  .Permit(SessionAction.Cancel, SessionState.Completed)
                  .OnEntryFrom(SessionAction.Complete, StartRecording)
                  .OnExit(StopRecording)
                  .OnExit(ClearStep);

            states.Configure(SessionState.Processing)
                  .InternalTransition(SessionAction.Continue, TakeNextStep)
                  .Permit(SessionAction.Complete, SessionState.Completed)
                  .Permit(SessionAction.Cancel, SessionState.Completed)
                  .OnEntryFrom(SessionAction.Complete, Process)
                  .OnExit(ClearStep);

            states.Configure(SessionState.Completed)
                  .Ignore(SessionAction.Open)
                  .Ignore(SessionAction.Continue)
                  .Ignore(SessionAction.Cancel)
                  .Ignore(SessionAction.Complete)
                  .OnEntry(OnCompleted);

            states.Configure(SessionState.Cancelled)
                  .Ignore(SessionAction.Open)
                  .Ignore(SessionAction.Continue)
                  .Ignore(SessionAction.Cancel)
                  .Ignore(SessionAction.Complete)
                  .OnEntry(OnCompleted);

            return states;
        }


        public SessionRecordingSettings Settings { get; }


        public string Id { get { return Settings.IdProperty.Get<string>(); } }


        public SessionDefinition Definition => _definition;


        public IDictionary<DeviceType, IDictionary<string, object>> DeviceConfigurations { get; }

        public IEnumerable<DeviceType> SelectedDevices => DeviceConfigurations.Keys;

        public IDictionary<string, IDictionary<string, object>> RecorderConfigurations { get; }

        public IEnumerable<string> SelectedRecorders => RecorderConfigurations.Keys;


        private static bool IsStepValid(SessionStep step)
        {
            return step != null && step.Action != null && step.Completion != null;
        }

        private static IEnumerable<SessionStep> ValidateSteps(IEnumerable<SessionStep> steps)
        {
            return steps?.Where(s => IsStepValid(s))
                ?? Enumerable.Empty<SessionStep>();
        }


        private static IEnumerable<SessionDeviceDefinition> ValidateDevices(IEnumerable<SessionDeviceDefinition> definitions)
        {
            var selection = definitions?.Where(d => d?.Device != null)
                                        .Distinct(SessionDeviceDefinition.ComparerByKey)
                ?? Enumerable.Empty<SessionDeviceDefinition>();

            //// if no local 
            //if (recorders.Contains("Local") == false)
            //{
            //    selection = selection.Where(d => DeviceType.StreamingDevices.Contains(d.Device) == false);
            //}

            return selection;
        }


        private static IEnumerable<SessionRecorderDefinition> ValidateRecorders(IEnumerable<SessionRecorderDefinition> definitions)
        {
            return definitions?.Where(d => String.IsNullOrWhiteSpace(d?.Name) == false)
                               .Distinct(SessionRecorderDefinition.ComparerByKey)
                ?? Enumerable.Empty<SessionRecorderDefinition>();
        }


        #region Open session

        public async Task<bool> OpenAsync(CancellationToken cancellationToken)
        {
            _adapters.ResetConfigurations();
            _adapters.Configure(DeviceConfigurations);

            if (_stateMachine.CanFire(SessionAction.Continue))
            {
                throw new InvalidOperationException($"Cannot open session in its current state {State.ToString()}.");
            }

            _stateMachine.Fire(SessionAction.Open);

            using (var cancellation = new CancellationDisposable())
            {
                _cancellation.Disposable = cancellation;

                try
                {
                    await _adapters.ConnectAsync(SelectedDevices, cancellation.Token);

                    if (_stateMachine.CanFire(SessionAction.Complete))
                    {
                        _stateMachine.Fire(SessionAction.Complete);

                        return true;
                    }
                    else
                    {
                        TryCancel();
                        return false;
                    }
                }
                catch
                {
                    TryCancel();
                    throw;
                }
            }

        }
        #endregion

        private bool CheckDeviceStates() => _definition.StrictStart == false
                                            || _adapters.CheckAreDevicesInState(DeviceState.Connected, SelectedDevices);

        public bool Continue()
        {
            if (_currentSteps != null && _currentSteps.Count > 0)
            {
                _stateMachine.Fire(SessionAction.Continue);
                return true;
            }
            else if (_stateMachine.CanFire(SessionAction.Complete))
            {
                _stateMachine.Fire(SessionAction.Complete);
                return true;
            }

            return false;
        }

        public bool Continue(SessionStep step)
        {
            if (IsStepValid(step))
            {
                CurrentStep = new SessionStepExecution(step, DateTime.Now);

                return Continue();
            }

            return false;
        }

        private void Prepare()
        {
            ResetCurrentTimeline(_preSteps);

            Continue();
        }

        #region Start session

        private void StartRecording()
        {
            StartedAt = DateTime.Now;

            var cancellation = new CancellationDisposable();
            _cancellation.Disposable = cancellation;

            _adapters.StartRecordingAsync(SelectedDevices, cancellation.Token).Forget();

            ResetCurrentTimeline(_steps);

            Continue();
        }

        private void StopRecording()
        {
            FinishedAt = DateTime.Now;

            _adapters.StopRecordingAsync(CancellationToken.None).Forget();
        }

        #endregion


        /// <summary>
        /// Inserts steps at the specified position in the current session timeline based on the session state (Preparing, Running, Processing). If the steps are inserted at the beginning of the timeline, they are executed after the current step.
        /// </summary>
        /// <param name="steps"></param>
        public IEnumerable<SessionStep> InsertSteps(IEnumerable<SessionStep> steps, int position = 0)
        {
            var timeline = _currentSteps;
            if (timeline != null)
            {
                var addedSteps = InsertSteps(timeline, steps, position);

                if (addedSteps.Any())
                {
                    _events.OnNext(new SessionRecordingTimelineChanged(addedSteps, State, position, DateTime.Now, State));
                }

                return addedSteps;
            }

            return Enumerable.Empty<SessionStep>();
        }


        /// <summary>
        /// Inserts steps at the specified position in the specified session timeline based on the given session state (expects one of these values: <seealso cref="SessionState.Preparing"/>, <seealso cref="SessionState.Running"/>, <seealso cref="SessionState.Processing"/>).
        /// </summary>
        /// <param name="targetTimeline"></param>
        /// <returns></returns>
        //public IEnumerable<SessionStep> InsertSteps(IEnumerable<SessionStep> steps, SessionState targetTimeline, int position = 0)
        //{
        //    var timeline = ResolveTimeline(ref targetTimeline);

        //    if (timeline != null)
        //    {
        //        var addedSteps = InsertSteps(timeline, steps, position);

        //        if (addedSteps.Any())
        //        {
        //            _events.OnNext(new SessionRecordingTimelineChanged(addedSteps, targetTimeline, position, DateTime.Now, State));
        //        }

        //        return addedSteps;
        //    }

        //    return Enumerable.Empty<SessionStep>();
        //}


        //private LinkedList<SessionStep> ResolveTimeline(ref SessionState targetTimeline)
        //{
        //    targetTimeline.ThrowIf(t => t.IsRunningState() == false, nameof(targetTimeline));

        //    LinkedList<SessionStep> timeline = null;
        //    switch (targetTimeline)
        //    {
        //        case SessionState.Preparing:
        //            timeline = _preSteps;
        //            break;
        //        case SessionState.Processing:
        //            timeline = _postSteps;
        //            break;
        //        default:
        //        case SessionState.Running:
        //            timeline = _steps;
        //            targetTimeline = SessionState.Running;
        //            break;
        //    }

        //    return timeline;
        //}


        private void ResetCurrentTimeline(LinkedList<SessionStep> timeline)
        {
            _currentSteps = timeline;
        }


        /// <summary>
        /// Inserts steps at the specified position in the timeline.
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="steps"></param>
        private List<SessionStep> InsertSteps(LinkedList<SessionStep> timeline, IEnumerable<SessionStep> steps, int position = 0)
        {
            timeline.ThrowIfNull(nameof(timeline));

            List<SessionStep> addedSteps = ValidateSteps(steps).ToList();

            int index = Math.Max(0, position);

            if (index >= timeline.Count)
            {
                addedSteps.ForEach(s => timeline.AddLast(s));
            }
            else
            {
                LinkedListNode<SessionStep> nextElement = timeline.First;
                while (position-- > 0)
                {
                    nextElement = nextElement.Next;
                }

                addedSteps.ForEach(s => timeline.AddBefore(nextElement, s));
            }

            return addedSteps;
        }


        private List<SessionStep> InsertStep(LinkedList<SessionStep> timeline, params SessionStep[] steps)
        {
            return InsertSteps(timeline, steps);
        }


        private void TakeNextStep()
        {
            var step = _currentSteps.First();
            _currentSteps.RemoveFirst();

            CurrentStep = new SessionStepExecution(step, DateTime.Now);
        }

        private void ClearStep()
        {
            CurrentStep = null;
        }


        public bool CanCancel() => _stateMachine.CanFire(SessionAction.Cancel);


        private void Process()
        {
            ResetCurrentTimeline(_postSteps);

            Continue();
        }


        private void OnCompleted()
        {
            _cancellation.Disposable = Disposable.Empty;

            _events.OnCompleted();
        }


        public bool TryCancel()
        {
            if (CanCancel())
            {
                _stateMachine.Fire(SessionAction.Cancel);
                return true;
            }

            return false;
        }


        private SessionState state = SessionState.None;
        public SessionState State
        {
            get { return state; }
            private set
            {
                var previous = ObjectEx.GetAndReplace(ref state, value);
                if (previous != value)
                {
                    StateChanged?.Invoke(this, new ValueChangedEventArgs<SessionState>(value, previous));
                    _events.OnNext(new SessionRecordingEvent(value, DateTime.Now));
                }
            }
        }

        public event EventHandler<ValueChangedEventArgs<SessionState>> StateChanged;


        public bool IsRunning => state.IsRunningState();

        public bool IsActive => state == SessionState.Opening
                             || state.IsRunningState();


        private SessionStepExecution currentStep = null;
        public SessionStepExecution CurrentStep
        {
            get { return currentStep; }
            private set
            {
                var previous = ObjectEx.GetAndReplace(ref currentStep, value);
                if (previous != value)
                {
                    if (previous != null)
                    {
                        if (previous.Result == null)
                        {
                            previous.Result = SessionStepResult.Skipped;
                        }
                        _events.OnNext(new SessionStepCompletedEvent(previous, DateTime.Now, State));
                    }

                    if (value != null)
                    {
                        CurrentStepChanged?.Invoke(this, new ValueChangedEventArgs<SessionStepExecution>(value, previous));
                        _events.OnNext(new SessionStepStartedEvent(value, State));
                    }
                }
            }
        }

        public event EventHandler<ValueChangedEventArgs<SessionStepExecution>> CurrentStepChanged;


        public DateTime OpenedAt
        {
            get { return Settings.OpenedAtProperty.Get<DateTime>(); }
        }


        private DateTime? startedAt;
        public DateTime? StartedAt
        {
            get { return startedAt; }
            set { startedAt = value; Settings.StartedAtProperty.Set(value); }
        }


        private DateTime? finishedAt;
        public DateTime? FinishedAt
        {
            get { return finishedAt; }
            set { finishedAt = value; Settings.FinishedAtProperty.Set(value); }
        }


        public IObservable<SessionRecordingEvent> Events => _events;


        public bool IsFinished => State == SessionState.Completed || State == SessionState.Cancelled;


        public ICollection<ISessionRecordingResult> Results { get; } = new List<ISessionRecordingResult>();


        #region IDisposable Members

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    TryCancel();

                    _events.Dispose();

                    //var recorders = _recorders.ToList();
                    //_recorders.Clear();
                    //recorders.OfType<IDisposable>().ForEach(r => r.Dispose());
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
