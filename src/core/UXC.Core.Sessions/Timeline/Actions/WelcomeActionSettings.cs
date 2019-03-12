﻿/**
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
    public class WelcomeActionSettings : QuestionaryActionSettings
    {
        public bool Ignore { get; set; } 

        public bool HideDescription { get; set; }

        public Text Description { get; set; }

        public bool HideDevices { get; set; }

        public string StartButtonLabel { get; set; }
    }
}
