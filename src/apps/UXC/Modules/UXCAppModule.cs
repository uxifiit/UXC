using UXC.ViewServices;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core;
using UXC.Sessions;
using UXC.Services;
//using UXC.Core.Services;
using UXC;
using UXC.Configuration;
using UXC.Core.ViewModels;
using UXC.ViewModels.Settings;

namespace UXC.Modules
{
    public class UXCAppModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISessionDefinitionsSource>().To<DefaultSessionDefinitions>().InSingletonScope();
            Bind<ISessionDefinitionsSource>().To<LocalSessionDefinitions>().InSingletonScope();

            if (Kernel.GetBindings(typeof(IAppConfiguration)).Any() == false)
            {
                Bind<IAppConfiguration>().To<AppConfiguration>().InSingletonScope();
            }

            Bind<ISettingsSectionViewModel>().To<AppSettingsSectionViewModel>().InSingletonScope();
        }
    }
}
