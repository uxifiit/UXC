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
using Ninject.Modules;
using UXC.Core;

namespace UXC.Core.Modules
{
    public class AppModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAppService>().To<AppService>().InSingletonScope();
        }
    }
}
