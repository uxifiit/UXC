using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Results;

namespace UXC.Sessions
{
    // TODO DECISION refactor SessionRecordingEvent to abstract class and create SessionRecordingStateEvent where EventType == SessionState::ToString().

    public class SessionRecordingEvent
    {
        public SessionRecordingEvent(SessionState state, DateTime timestamp)
        {
            State = state;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Gets the state of the SessionRecording when this event occurred.
        /// </summary>
        public SessionState State { get; }

        public DateTime Timestamp { get; }

        public virtual string EventType => "StateChanged";
    }


    public class SessionStepStartedEvent : SessionRecordingEvent
    {
        public SessionStepStartedEvent(SessionStepExecution execution, SessionState state)
            : base(state, execution.StartedAt)
        {
            StepAction = execution.Step.Action?.ActionType;
        }

        public string StepAction { get; }

        public override string EventType => "StepStarted";
    }


    public class SessionStepCompletedEvent : SessionRecordingEvent
    {
        public SessionStepCompletedEvent(SessionStepExecution execution, DateTime timestamp, SessionState state)
            : base(state, timestamp)
        {
            Step = execution.Step;
            Result = execution.Result;
            StartedAt = execution.StartedAt;
        }

        public SessionStep Step { get; }

        public SessionStepResult Result { get; }

        public DateTime StartedAt { get; }

        public override string EventType => "StepCompleted";
    }

    //public class SessionCompletedEvent : SessionRecordingEvent
    //{
    //    public SessionCompletedEvent(/*IEnumerable<ISessionRecorder> recorders, */DateTime timestamp)
    //        : base(SessionState.Completed, timestamp)
    //    {
    //        //Recorders = recorders;
    //    }
    //    //public IEnumerable<ISessionRecorder> Recorders { get; }
    //}


    //public class SessionCancelledEvent : SessionRecordingEvent
    //{
    //    public SessionCancelledEvent(DateTime timestamp) // add some reason why the session was cancelled?
    //        : base(SessionState.Cancelled, timestamp)
    //    { }
    //}
}
