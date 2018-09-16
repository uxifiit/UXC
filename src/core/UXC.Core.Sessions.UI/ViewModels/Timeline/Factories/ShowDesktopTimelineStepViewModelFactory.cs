using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Modules;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Executors;

namespace UXC.Sessions.ViewModels.Timeline.Factories
{
    class ShowDesktopTimelineStepViewModelFactory : RelayViewModelFactory<ShowDesktopActionSettings, ITimelineStepViewModel>
    {
        public ShowDesktopTimelineStepViewModelFactory(IInstanceResolver resolver)
            : base(settings => new ExecutedTimelineStepViewModel(settings, new ShowDesktopStepActionExecutor(), resolver.Get<ViewModelResolver>()))
        {
        }
    }
}
