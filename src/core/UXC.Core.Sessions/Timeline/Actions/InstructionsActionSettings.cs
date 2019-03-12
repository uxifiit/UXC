/**
 * UXC.Core.Sessions
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System.Collections.Generic;

namespace UXC.Sessions.Timeline.Actions
{
    public class InstructionsActionSettings : ContentActionSettingsBase
    {
        public string Title { get; set; }

        public Text Instructions { get; set; }

        public List<string> Parameters { get; set; } = null;

        public bool ShowContinue { get; set; }

        public string ContinueButtonLabel { get; set; }
    }
}
