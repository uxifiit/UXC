using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using UXC.Core.ViewModels;
using UXC.Plugins.Sessions.Fixations.ViewModels;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Plugins.Sessions.Fixations
{
    public class SessionFixationsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IViewModelFactory>().To<FixationFilterTimelineStepViewModelFactory>().InSingletonScope();

            RegisterTimelineSteps();
        }

        private void RegisterTimelineSteps()
        {
            SessionStepActionSettings.Register(typeof(FixationFilterActionSettings));
        }
    }
}
