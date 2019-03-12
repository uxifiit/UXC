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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using UXC.Core.Plugins.Managers;

namespace UXC.Core.Plugins.Loaders
{
    class AssemblyPluginsLoader : ILoader
    {
        private readonly PluginsManager _manager;
        private readonly IPluginsService _service;

        private static readonly Type ModuleInterfaceType = typeof(INinjectModule);

        internal AssemblyPluginsLoader(PluginsManager manager, IPluginsService service)
        {
            _manager = manager;
            _service = service;
        }

        public void Load()
        {
            var files = _service.GetPluginFiles();
            if (files.Any())
            {
                foreach (var file in files.Where(f => File.Exists(f)))
                {
                    var assembly = Assembly.LoadFile(file);
                    var modules = assembly.GetExportedTypes()
                                          .Where(t => t.IsAbstract == false 
                                                   && t.GetInterfaces().Contains(ModuleInterfaceType))
                                          .ToArray();

                    foreach (var module in modules)
                    {
                        Plugin plugin = new Plugin();
                        plugin.Assembly = assembly.GetName().FullName;
                        plugin.Module = (INinjectModule)Activator.CreateInstance(module);
                        plugin.Version = assembly.GetName().Version;

                        _manager.Connect(plugin);
                    }
                }
            }
        }
    }
}
