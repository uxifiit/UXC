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
using UXC.Configuration;
using UXI.OwinServer;
using UXC.Core;
using UXC.Services;
using UXC.Core.ViewModels;
using UXC.ViewModels.Settings;

namespace UXC.Modules
{
    public class OwinServerModule : NinjectModule
    {
        public override string Name
        {
            get
            {
                return "Server";
            }
        }

        public override void Load()
        {
            Bind<Microsoft.AspNet.SignalR.Infrastructure.IProtectedData>().To<Server.ProtectedData>();

            if (Kernel.GetBindings(typeof(IServerConfiguration)).Any() == false)
            {
                Bind<IServerConfiguration>().To<ServerConfiguration>().InSingletonScope();
            }

            Bind<ServerHost>().ToSelf().WhenInjectedExactlyInto<ServerControlService>().InSingletonScope();

            Bind<IControlService, ServerControlService>().To<ServerControlService>().InSingletonScope();

            Bind<ISettingsSectionViewModel>().To<ServerSettingsSectionViewModel>().InSingletonScope();
        }
    }
}
