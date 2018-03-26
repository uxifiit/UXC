using System;
using System.Threading;
using System.Threading.Tasks;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;

namespace UXC.Sessions.Timeline.Executors
{
    public interface ISessionStepActionExecutor
    {
        bool CanExecute(SessionStepActionSettings settings);

        void Execute(SessionRecording recording, SessionStepActionSettings settings);

        SessionStepResult Complete();

        event EventHandler<SessionStepResult> Completed;
    }
}
