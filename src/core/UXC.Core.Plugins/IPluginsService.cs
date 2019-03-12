/**
 * UXC.Core.Plugins
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System.Collections.Generic;
using Ninject.Modules;

namespace UXC.Core.Plugins
{
    public interface IPluginsService
    {
        IEnumerable<string> GetPluginFiles();
    }
}
