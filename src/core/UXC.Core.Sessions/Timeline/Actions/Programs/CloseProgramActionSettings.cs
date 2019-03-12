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

namespace UXC.Sessions.Timeline.Actions
{
    public class CloseProgramActionSettings : ExecutedActionSettingsBase
    {
        public bool ForceClose { get; set; }

        public TimeSpan? ForceCloseTimeout { get; set; }

        public string Message { get; set; }
    }
}
