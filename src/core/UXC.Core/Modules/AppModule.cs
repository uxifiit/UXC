using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using UXC;
using UXC.Core.ViewServices;

namespace UXC.Core.Modules
{
    public class AppModule : NinjectModule
    {
        public override void Load()
        {
           /// TODO add configurations

            Bind<IAppService>().To<AppService>().InSingletonScope();
        }
    }
}
