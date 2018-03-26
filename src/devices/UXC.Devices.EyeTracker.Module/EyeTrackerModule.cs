using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using UXC.Core.Devices;
using UXC.Devices.EyeTracker.Configuration;

namespace UXC.Devices.EyeTracker
{
    public class EyeTrackerModule : NinjectModule
    {
        public override void Load()
        {
            if (Kernel.GetBindings(typeof(IEyeTrackerConfiguration)).Any() == false)
            {
                Bind<IEyeTrackerConfiguration>().To<EyeTrackerConfiguration>().InSingletonScope();
            }

            Kernel.Bind(x => x.FromAssembliesMatching("UXC.Devices.EyeTracker.Driver.*.dll")
                              .SelectAllClasses()
                              .InheritedFromAny(typeof(ITrackerFinder))
                              .BindAllInterfaces()
                              .Configure(a => /*a.WhenInjectedExactlyInto(typeof(TrackerBrowser))*/
                                                a.InSingletonScope()));

            Bind<TrackerBrowser>().ToSelf().WhenInjectedExactlyInto<EyeTrackerDevice>().InSingletonScope();

            Bind<IDevice>().To<EyeTrackerDevice>().InSingletonScope();
        }
    }
}
