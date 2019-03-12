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
using UXI.Common;
using UXC.Core.Modules;
using UXC.Core.Plugins.Configuration;

namespace UXC.Core.Plugins
{
    public class PluginsService : IPluginsService
    {

        private readonly IPluginsConfiguration _configuration;

        public PluginsService(IPluginsConfiguration configuration, string directory)
        {
            _configuration = configuration;
            _directory = directory;
        }

        private const string DEFAULT_PLUGINS_SELECTION = "*";
        private readonly string _directory;

        public IEnumerable<string> GetPluginFiles()
        {
            string pluginsSelection = _configuration.Plugins?.Trim() ?? DEFAULT_PLUGINS_SELECTION; // TODO move to config? 

            string pluginsPath = Path.Combine(Locations.ExecutingAssemblyLocationPath, _directory);
            string directory = Path.GetDirectoryName(pluginsPath);

            if (Directory.Exists(directory) == false)
            {
                return Enumerable.Empty<string>();
            }

            string[] files = null;

            if (String.IsNullOrWhiteSpace(pluginsSelection) || pluginsSelection.Equals(DEFAULT_PLUGINS_SELECTION))
            {
                files = Directory.GetFiles(directory);
            }
            else
            {
                string[] filenames = pluginsSelection.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (filenames != null && filenames.Any())
                {
                    files = filenames
                        .Select(f => Path.Combine(pluginsPath, f))
                        .Where(f => Path.GetExtension(f).Equals(PluginsConstants.PLUGIN_EXTENSION))
                        .Where(f => File.Exists(f))
                        .ToArray();
                }
            }

            return files ?? Enumerable.Empty<string>();
        }
    }
}
