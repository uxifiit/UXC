using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Executors;

namespace UXC.Sessions.ViewModels.Timeline.Factories
{
    class CloseProgramTimelineStepViewModelFactory : RelayViewModelFactory<CloseProgramActionSettings, ITimelineStepViewModel>
    {
        public CloseProgramTimelineStepViewModelFactory(IProcessService service)
            : base(settings => new ExecutedTimelineStepViewModel(settings, new CloseProgramActionExecutor(service)))
        {
        }
    }
}
