/**
 * UXC.Core.Sessions.UI
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
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.ViewModels.Timeline.Factories
{
    class DefaultTimelineStepViewModelFactory : RelayViewModelFactory<SessionStepActionSettings, ITimelineStepViewModel>
    {
        public DefaultTimelineStepViewModelFactory()
            : base(_ => new TimelineStepViewModel())
        {
        }
    }
}
