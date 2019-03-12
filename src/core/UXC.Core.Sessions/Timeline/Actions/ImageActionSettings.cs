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

namespace UXC.Sessions.Timeline.Actions
{
    public class ImageActionSettings : ContentActionSettingsBase
    {
        public string Path { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public bool Stretch { get; set; }

        public bool ShowContinue { get; set; }

        public string ContinueButtonLabel { get; set; }
    }
}
