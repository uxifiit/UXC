/**
 * UXC.Devices.KeyboardMouse.Module
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
using UXC.Devices;
using UXC.Core.Devices;

namespace UXC.Devices.KeyboardMouse.Module
{
    public class KeyboardModule : NinjectModule
    {
        public override void Load()
        {
            //if (Kernel.GetBindings(typeof(IKeyboardConfiguration)).Any() == false)
            //{
            //    Bind<IKeyboardConfiguration>().To<KeyboardConfiguration>().InSingletonScope();
            //}

            Bind<IDevice>().To<KeyboardDevice>().InSingletonScope();

        }
    }

    public class MouseModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDevice>().To<MouseDevice>().InSingletonScope();
        }
    }
}
