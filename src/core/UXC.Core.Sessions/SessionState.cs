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

namespace UXC.Sessions
{
    public enum SessionState
    {
        None,

        Opening,

        Preparing,

        Running,

        Processing,

        Completed,

        Cancelled
    }


    public static class SessionStateEx
    {
        public static bool IsRunningState(this SessionState state)
        {
            return state == SessionState.Preparing
                || state == SessionState.Running
                || state == SessionState.Processing;
        }
    }
}
