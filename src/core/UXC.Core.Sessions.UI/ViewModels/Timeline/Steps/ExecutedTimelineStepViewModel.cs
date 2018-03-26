using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Executors;
using UXC.Sessions.Timeline.Results;
using UXI.Common.Extensions;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels.Timeline
{
    public class ExecutedTimelineStepViewModel : ITimelineStepViewModel
    {
        private readonly ISessionStepActionExecutor _executor;
        private readonly SessionStepActionSettings _settings;

        public ExecutedTimelineStepViewModel(SessionStepActionSettings settings, ISessionStepActionExecutor executor)
        {
            executor.ThrowIfNull(nameof(executor));
            executor.ThrowIf(e => e.CanExecute(settings) == false, 
                nameof(executor), 
                $"Given executor '{executor.GetType().Name}' can not execute settings of type '{settings?.GetType().Name ?? String.Empty}'.");

            _executor = executor;
            _executor.Completed += (_, args) => Dispatcher.CurrentDispatcher.Invoke(() => Completed?.Invoke(this, args));

            _settings = settings;
        }


        public bool IsContent => false;


        public event EventHandler<SessionStepResult> Completed;


        public SessionStepResult Complete()
        {
            return _executor.Complete();
        }


        public void Execute(SessionRecordingViewModel recording)
        {
            _executor.Execute(recording.Recording, _settings);
        }
    }
}
