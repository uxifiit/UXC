using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels.Timeline
{
    public interface ITimelineStepViewModel
    {
        void Execute(SessionRecordingViewModel recording);

        SessionStepResult Complete();

        event EventHandler<SessionStepResult> Completed;

        bool IsContent { get; }

        event EventHandler<bool> IsContentChanged;
    }
}
