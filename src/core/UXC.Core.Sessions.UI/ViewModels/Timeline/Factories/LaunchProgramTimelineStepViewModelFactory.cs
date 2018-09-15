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
    class LaunchProgramTimelineStepViewModelFactory : RelayViewModelFactory<LaunchProgramActionSettings, ITimelineStepViewModel>
    {
        public LaunchProgramTimelineStepViewModelFactory(IProcessService service)
            : base(settings => new ExecutedTimelineStepViewModel(settings, new LaunchProgramActionExecutor(service)))
        {
        }
    }
}
