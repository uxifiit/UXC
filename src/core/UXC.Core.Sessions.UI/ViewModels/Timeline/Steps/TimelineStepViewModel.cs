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
using UXC.Sessions.Timeline.Results;

namespace UXC.Sessions.ViewModels.Timeline.Factories
{
    class TimelineStepViewModel : ITimelineStepViewModel
    {
        public event EventHandler<SessionStepResult> Completed { add { } remove { } }

        public SessionStepResult Complete()
        {
            return SessionStepResult.Successful;
        }

        public void Execute(SessionRecordingViewModel recording)
        {
        }

        public bool IsContent => true;

        public event EventHandler<bool> IsContentChanged { add { } remove { } }
    }
}
