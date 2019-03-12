/**
 * UXC.Core.Plugins
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;

namespace UXC.Core.Plugins
{
    [Flags]
    public enum PluginsLoadingMode
    {
        Implicit = 1,
        Explicit = 2,
        ImplicitAndExplicit = Implicit | Explicit
    }
}
