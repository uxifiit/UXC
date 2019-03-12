/**
 * GazeVisualization
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
using GazeVisualization.Observers;
using GazeVisualization.Services;
using GazeVisualization.ViewModels;
using Ninject.Modules;
using UXC.Core;
using UXC.Core.ViewModels;
using UXC.Observers;


namespace GazeVisualization
{
    public class GazeVisualizationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDeviceObserver, GazeObserver>().To<GazeObserver>().InSingletonScope();
            Bind<IControlService>().To<GazeDisplayControlService>().InSingletonScope();

            Bind<IViewModelFactory>().To<GazeDisplayControlServiceIconViewModelFactory>()
                                     .WhenInjectedExactlyInto<ViewModelResolver>().InSingletonScope();
        }
    }
}
