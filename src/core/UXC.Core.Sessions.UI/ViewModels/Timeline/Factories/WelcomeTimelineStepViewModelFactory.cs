using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Managers;
using UXC.Core.Modules;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.ViewModels.Timeline.Factories
{
    class WelcomeTimelineStepViewModelFactory : RelayViewModelFactory<WelcomeActionSettings, ITimelineStepViewModel>
    {
        public WelcomeTimelineStepViewModelFactory(IAdaptersManager adapters, IInstanceResolver resolver)
            : base(settings => new WelcomeTimelineStepViewModel(settings, adapters, resolver.Get<ViewModelResolver>()))
        {
        }
    }
}
