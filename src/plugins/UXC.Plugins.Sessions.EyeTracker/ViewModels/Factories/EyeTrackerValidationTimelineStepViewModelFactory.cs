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

namespace UXC.Sessions.ViewModels.Timeline.Factories
{
    class EyeTrackerValidationTimelineStepViewModelFactory : RelayViewModelFactory<EyeTrackerValidationActionSettings, ITimelineStepViewModel>
    {
        public EyeTrackerValidationTimelineStepViewModelFactory()
            : base(settings => new EyeTrackerValidationTimelineStepViewModel(settings))
        {
        }
    }
}
