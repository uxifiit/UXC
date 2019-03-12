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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using UXC.Core.Plugins.Managers;

namespace UXC.Core.Plugins.Loaders
{
    class ExplicitPluginsLoader : ILoader
    {
        private readonly PluginsManager _manager;
        private readonly IEnumerable<INinjectModule> _modules;

        internal ExplicitPluginsLoader(PluginsManager manager, IEnumerable<INinjectModule> plugins)
        {
            _manager = manager;
            _modules = plugins?.ToList() ?? Enumerable.Empty<INinjectModule>();
        }

        public void Load()
        {
            foreach (var module in _modules)
            {
                var assembly = module.GetType().Assembly;

                Plugin plugin = new Plugin();
                plugin.Assembly = assembly.GetName().FullName;
                plugin.Module = module;
                plugin.Version = assembly.GetName().Version;

                _manager.Connect(plugin);
            }
        }
    }
}
