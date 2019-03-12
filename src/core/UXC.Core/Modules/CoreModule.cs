/**
 * UXC.Core
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
using Ninject;
using Ninject.Modules;
using UXC.Observers;
using UXC.Core.Managers;
using UXC.Core.Managers.Adapters;
using UXC.Models.Contexts;
using UXC.Core.Logging;

namespace UXC.Core.Modules
{
    public class CoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IInstanceResolver)).To(typeof(InstanceResolver)).InSingletonScope();

            Bind<IModulesService, IModulesNotifier>().To<ModulesService>().InSingletonScope();
            Bind<ModulesManager>().ToSelf().InSingletonScope();

            Bind<IDevicesContext, DevicesContext>().To<DevicesContext>().InSingletonScope();

            // clients
            Bind<IDeviceObserver, DevicesStatesWatcher>().To<DevicesStatesWatcher>().InSingletonScope();

            // managers
            Bind<IAdaptersManager>().To<AdaptersManager>().InSingletonScope();
            Bind<IAdaptersControl>().To<AdaptersControl>().InSingletonScope();
            Bind<IObserversManager>().To<ObserversManager>().InSingletonScope();

            Bind<IConnector>().To<DevicesConnector>();
            Bind<IConnector>().To<ObserversConnector>();

            Bind<ILogger>().To<NullLogger>().InSingletonScope();
        }
    }
}
