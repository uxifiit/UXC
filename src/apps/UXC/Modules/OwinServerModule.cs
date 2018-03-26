using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using UXC.Configuration;
using UXI.OwinServer;
using UXC.Core;
using UXC.Services;
using UXC.Core.ViewModels;
using UXC.ViewModels.Settings;

namespace UXC.Modules
{
    public class OwinServerModule : NinjectModule
    {
        public override string Name
        {
            get
            {
                return "Server";
            }
        }

        public override void Load()
        {
            if (Kernel.GetBindings(typeof(IServerConfiguration)).Any() == false)
            {
                Bind<IServerConfiguration>().To<ServerConfiguration>().InSingletonScope();
            }

            Bind<ServerHost>().ToSelf().WhenInjectedExactlyInto<ServerControlService>().InSingletonScope();

            Bind<IControlService, ServerControlService>().To<ServerControlService>().InSingletonScope();

            Bind<ISettingsSectionViewModel>().To<ServerSettingsSectionViewModel>().InSingletonScope();
        }
    }
}
