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
using Ninject.Modules;
using UXC.Core.ViewModels;
using UXC.Core.ViewModels.Adapters;
using UXC.ViewModels;

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
