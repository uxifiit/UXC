using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using UXC.Observers;
using UXC.Plugins.GazeAccess.Hubs;
using UXC.Plugins.GazeAccess.Controllers;
using UXC.Plugins.GazeAccess.Observers;

namespace UXC.Plugins.GazeAccess
{
    public class GazeAccessModule : NinjectModule
    {
        public override string Name
        {
            get
            {
                return "GazeAccess";
            }
        }


        public override void Load()
        {
            Bind<IDeviceObserver, EyeTrackerObserver>().To<EyeTrackerObserver>().InSingletonScope();

            Bind<GazeDataController>().ToSelf();

            Bind<HubClients>().ToSelf().WhenInjectedExactlyInto<GazeHub>().InSingletonScope();
            Bind<GazeHub>().ToSelf();
        }
    }
}
