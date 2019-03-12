/**
 * UXC.Plugins.Sessions.Fixations
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
using UXC.Plugins.Sessions.Fixations.ViewModels;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Plugins.Sessions.Fixations
{
    public class SessionFixationsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IViewModelFactory>().To<FixationFilterTimelineStepViewModelFactory>().InSingletonScope();

            RegisterTimelineSteps();
        }

        private void RegisterTimelineSteps()
        {
            SessionStepActionSettings.Register(typeof(FixationFilterActionSettings));
        }
    }
}
