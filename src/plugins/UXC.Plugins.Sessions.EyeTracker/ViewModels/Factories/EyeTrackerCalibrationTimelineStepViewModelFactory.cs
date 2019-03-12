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
using Ninject;
using UXC.Core.Managers;
using UXC.Core.Modules;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.ViewModels.Timeline;

namespace UXC.Sessions.ViewModels.Timeline.Factories
{
    class EyeTrackerCalibrationTimelineStepViewModelFactory : RelayViewModelFactory<EyeTrackerCalibrationActionSettings, ITimelineStepViewModel>
    {
        public EyeTrackerCalibrationTimelineStepViewModelFactory(IAdaptersManager adapters, IInstanceResolver resolver)
            : base(settings => new EyeTrackerCalibrationTimelineStepViewModel(settings, adapters, resolver.Get<ViewModelResolver>()))
        {
        }
    }
}
