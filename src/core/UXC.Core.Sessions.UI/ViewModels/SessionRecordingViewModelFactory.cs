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
using System.Windows.Threading;
using UXC.Core.Modules;
using UXC.Core.ViewModels;
using UXC.Core.ViewServices;
using UXC.Sessions.ViewModels.Timeline.Preparation;

namespace UXC.Sessions.ViewModels
{
    public class SessionRecordingViewModelFactory : RelayViewModelFactory<SessionRecording, SessionRecordingViewModel>
    {
        public SessionRecordingViewModelFactory(IInstanceResolver resolver, Dispatcher dispatcher)
            : base(recording => new SessionRecordingViewModel(recording, resolver.Get<ViewModelResolver>(), dispatcher, resolver.Get<TimelinePreparation>()))
        {
        }   
    }
}
