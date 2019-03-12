/**
 * UXC.Plugins.Sessions.EyeTracker
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
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.ViewModels.Timeline.Factories;

namespace UXC.Plugins.Sessions.Fixations
{
    public class SessionEyeTrackerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IViewModelFactory>().To<EyeTrackerCalibrationTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<EyeTrackerValidationTimelineStepViewModelFactory>().InSingletonScope();

            RegisterTimelineSteps();
        }

        private void RegisterTimelineSteps()
        {
            SessionStepActionSettings.Register(typeof(EyeTrackerCalibrationActionSettings));
            SessionStepActionSettings.Register(typeof(EyeTrackerValidationActionSettings));
        }
    }
}
