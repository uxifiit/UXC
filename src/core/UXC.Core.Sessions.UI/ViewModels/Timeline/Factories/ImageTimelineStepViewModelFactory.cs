using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using UXC.Core.ViewModels;
using UXC.Sessions.Models;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.ViewModels.Timeline.Factories
{
    class ImageTimelineStepViewModelFactory : RelayViewModelFactory<ImageActionSettings, ITimelineStepViewModel>
    {
        public ImageTimelineStepViewModelFactory(IImageService images, Dispatcher dispatcher) 
            : base(settings => new ImageTimelineStepViewModel(settings, images, dispatcher))
        {
        }
    }
}
