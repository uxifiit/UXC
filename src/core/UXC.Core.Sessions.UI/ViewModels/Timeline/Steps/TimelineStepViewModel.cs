using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Sessions.Timeline.Results;

namespace UXC.Sessions.ViewModels.Timeline.Factories
{
    class TimelineStepViewModel : ITimelineStepViewModel
    {
        public event EventHandler<SessionStepResult> Completed { add { } remove { } }

        public SessionStepResult Complete()
        {
            return SessionStepResult.Successful;
        }

        public void Execute(SessionRecordingViewModel recording)
        {
        }

        public bool IsContent => true;

        public event EventHandler<bool> IsContentChanged { add { } remove { } }
    }
}
