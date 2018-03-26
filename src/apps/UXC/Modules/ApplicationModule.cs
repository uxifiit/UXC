using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using UXC.Configuration;
using Ninject.Modules;
using UXC;
using UXI.Configuration;
using UXC.Models.Contexts;
using UXC.Models.Contexts.Design;
//using UXC.ViewServices;
using UXC.Core.ViewServices;
using Ninject;
using UXI.Configuration.Storages;
using UXC.Configuration.Design;
using UXI.App;
using UXC.Design.Configuration;

namespace UXC.Modules
{
    public class DesignApplicationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Dispatcher>().ToMethod(_ => Dispatcher.CurrentDispatcher);
            Bind<IConfigurationSource>().To<DesignConfigurationSource>().InSingletonScope();
            Bind<IAppConfiguration>().To<DesignAppConfiguration>().InSingletonScope();

            Bind<IAppContext>().To<DesignAppContext>().InSingletonScope();
        }
    }

    public class ApplicationModule : NinjectModule
    {
        public ApplicationModule()
        {
        }

        public override void Load()
        {
            // init
            Bind<Dispatcher>().ToConstant(System.Windows.Application.Current.Dispatcher);

            // configuration init
            Bind<IStorageLoader>().To<IniFileLoader>().InSingletonScope();
            Bind<IConfigurationSource>().To<ConfigurationSource>().InSingletonScope();

            Bind<IAppContext>().To<AppContext>().InSingletonScope();
          
            Bind<CommandLineOptionsParser>().ToSelf().InSingletonScope();
        }
    }
}
