using UXC.ViewModels;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.ViewModels;
using UXC.Core.ViewModels.Adapters;
using UXC.Devices.EyeTracker.ViewModels;
using UXC.Sessions.ViewModels;

namespace UXC.Modules
{
    class ViewModelsModule : NinjectModule
    {
        public override void VerifyRequiredModulesAreLoaded()
        {
            if (Kernel.HasModule(typeof(ViewServicesModule).FullName) == false)
            {
                throw new InvalidOperationException($"Module {typeof(ViewServicesModule).FullName} is required");
            }
        }

        public override void Load()
        {
            Bind<IViewModelFactory>().To<AdapterViewModelFactory>().InSingletonScope();
            Bind<ViewModelResolver>().ToSelf().InSingletonScope();

            Bind<AppViewModel>().ToSelf().InSingletonScope();
            Bind<AdaptersViewModel>().ToSelf().InSingletonScope();
            Bind<AboutViewModel>().ToSelf().InSingletonScope();
            Bind<SettingsViewModel>().ToSelf().InSingletonScope();
        }
    }
}
