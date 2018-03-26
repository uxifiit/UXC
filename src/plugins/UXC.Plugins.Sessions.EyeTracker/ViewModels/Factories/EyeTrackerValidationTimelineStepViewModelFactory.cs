using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using UXC.Core.Managers;
using UXC.Core.Modules;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.ViewModels.Timeline.Factories
{
    class EyeTrackerValidationTimelineStepViewModelFactory : RelayViewModelFactory<EyeTrackerValidationActionSettings, ITimelineStepViewModel>
    {
        public EyeTrackerValidationTimelineStepViewModelFactory()
            : base(settings => new EyeTrackerValidationTimelineStepViewModel(settings))
        {
        }
    }
}
