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
using UXC.Core.Data;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.Timeline
{
    public class SessionStep
    {
        public readonly static SessionStep Default = new SessionStep()
        {
            Action = new ShowDesktopActionSettings(),
            Completion = SessionStepCompletionSettings.Default
        };


        public SessionStepActionSettings Action { get; set; }

        public SessionStepCompletionSettings Completion { get; set; } = new SessionStepCompletionSettings();

        public SessionStep Clone()
        {
            return new SessionStep()
            {
                Action = Action?.Clone(),
                Completion = Completion?.Clone()
            };
        }
    }
  

    public class SessionStepCompletionSettings
    {
        public readonly static SessionStepCompletionSettings Default = new SessionStepCompletionSettings();

        public TimeSpan? Duration { get; set; } = null;

        public List<Hotkey> Hotkeys { get; set; } = new List<Hotkey>();

        public SessionStepCompletionSettings Clone()
        {
            var clone = (SessionStepCompletionSettings)this.MemberwiseClone();

            clone.Hotkeys = Hotkeys?.ToList() ?? new List<Hotkey>();

            return clone;
        }
    }

}
