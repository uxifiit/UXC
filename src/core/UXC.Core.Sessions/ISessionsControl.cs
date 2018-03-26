using System;
using System.Threading;
using System.Threading.Tasks;

namespace UXC.Sessions
{
    public interface ISessionsControl
    {
        SessionRecording CurrentRecording { get; }
        IObservable<SessionRecording> Recordings { get; }

        IObservable<ISessionRecordingResult> CompletedRecordings { get; }

        event EventHandler<SessionRecording> RecordingChanged;

        void Close();

        void Clear();

        SessionRecording Record(SessionDefinition definition);
    }
}
