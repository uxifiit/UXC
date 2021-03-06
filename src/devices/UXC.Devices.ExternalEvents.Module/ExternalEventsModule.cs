/**
 * UXC.Devices.ExternalEvents.Module
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
using UXC.Core.Devices;
using UXC.Devices.ExternalEvents.Controllers;
using UXC.Devices.ExternalEvents.Hubs;
using UXC.Devices.ExternalEvents.Services;

namespace UXC.Devices.ExternalEvents.Module
{
    public class ExternalEventsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEventsRecorder>().To<EventsRecorder>()
                .WhenInjectedInto
                (
                    typeof(EventService), 
                    typeof(ExternalEventsDevice)
                )
                .InSingletonScope();

            Bind<EventService>().ToSelf()
                                .WhenInjectedExactlyInto(typeof(EventController), typeof(EventHub))
                                .InSingletonScope();

            Bind<EventController>().ToSelf();
            Bind<EventHub>().ToSelf();

            Bind<IDevice>().To<ExternalEventsDevice>().InSingletonScope();
        }
    }
}
