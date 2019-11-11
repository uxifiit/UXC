/**
 * UXC
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Ninject;
using Ninject.Modules;
using UXC;
using UXC.Configuration;
using UXI.Configuration;
using UXC.Models.Contexts;
using UXC.Models.Contexts.Design;
using UXI.Configuration.Storages;
using UXC.Configuration.Design;
using UXC.Design.Configuration;
using UXC.Common;

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
