using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.ViewModels.Timeline.Factories
{
    class DefaultTimelineStepViewModelFactory : RelayViewModelFactory<SessionStepActionSettings, ITimelineStepViewModel>
    {
        public DefaultTimelineStepViewModelFactory()
            : base(_ => new TimelineStepViewModel())
        {
        }
    }
}
