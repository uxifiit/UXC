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
    public abstract class ContentActionSettingsBase : SessionStepActionSettings
    {
        public string Background { get; set; }
        public string Foreground { get; set; }
        public int? FontSize { get; set; }
        public virtual bool? ShowCursor { get; set; }
    }
}
