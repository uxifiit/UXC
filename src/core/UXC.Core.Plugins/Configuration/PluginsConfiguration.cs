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
using UXI.Common;
using UXI.Configuration;
using UXI.Configuration.Attributes;
using UXI.Configuration.Storages;
using UXC.Core.Plugins.Configuration;

namespace UXC.Core.Plugins.Configuration
{
    [ConfigurationSection("Plugins")]
    internal class PluginsConfiguration : ConfigurationBase, IPluginsConfiguration
    {

        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield return StorageDefinition.Create("plugins.ini");//Locations.CallingAssemblyLocationName;
            }
        }


        public PluginsConfiguration(IConfigurationSource source) : base(source)
        {
            PluginsProperty = CreateProperty(nameof(Plugins), DEFAULT_Plugins, source);
        }

        public IEnumerable<IConfigurationSettingProperty> Properties
        {
            get { yield return PluginsProperty; }
        }

        private const string DEFAULT_Plugins = "*";
        [global::System.Configuration.Setting]
        [global::System.Diagnostics.DebuggerNonUserCode]
        //[global::System.Configuration.DefaultSettingValue("*")]
        public string Plugins
        {
            get
            {
                return PluginsProperty.Get<string>();
            }
            set
            {
                PluginsProperty.Set(value);
            }
        }

        private readonly ConfigurationSettingProperty PluginsProperty;
    }
}
