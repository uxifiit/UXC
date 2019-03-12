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
using System.Windows;
using UXC.Core.ViewServices;

namespace UXC.Modules
{
    internal class ViewServicesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<INavigationService>().ToConstant(new ContentFrameNavigationService(new DependencyObjectProvider(() => Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()), "contentFrame"));

            Bind<IViewsService>().To<ViewsService>().InSingletonScope()
                .WithConstructorArgument(type: typeof(DependencyObjectProvider), value: new DependencyObjectProvider(() => Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()));

            Bind<ITrayIconProvider>().ToMethod(_ => Application.Current as ITrayIconProvider);

            Bind<IInteractionService>().To<InteractionService>().InSingletonScope();
            Bind<INotificationService>().To<NotificationService>().InSingletonScope();
        }
    }
}
