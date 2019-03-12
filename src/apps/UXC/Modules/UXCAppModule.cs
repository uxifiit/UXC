/**
 * UXC
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using UXC.ViewServices;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Sessions;
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
