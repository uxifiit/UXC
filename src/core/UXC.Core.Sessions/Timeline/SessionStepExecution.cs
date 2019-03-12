/**
 * UXC.Core.Sessions
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

namespace UXC.Sessions.Timeline
{
    public class SessionStepExecution
    {
        internal SessionStepExecution(SessionStep step, DateTime startedAt)
        {
            //Index = index;
            Step = step;
            StartedAt = startedAt;
        }

        //public int Index { get; }

        public SessionStep Step { get; }

        public DateTime StartedAt { get; }

        public SessionStepResult Result { get; set; }
    }
}
