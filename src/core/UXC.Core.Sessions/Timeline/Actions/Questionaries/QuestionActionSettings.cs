/**
 * UXC.Core.Sessions
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
namespace UXC.Sessions.Timeline.Actions
{
    public abstract class QuestionActionSettings : SessionStepActionSettings
    {
        public string Id { get; set; }

        public Text Question { get; set; }

        public bool IsRequired { get; set; } = true;

        public string HelpText { get; set; }

        public override SessionStepActionSettings Clone()
        {
            return (QuestionActionSettings)this.MemberwiseClone();
        }
    }
}
