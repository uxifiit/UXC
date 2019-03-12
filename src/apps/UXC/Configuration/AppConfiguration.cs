/**
 * UXC
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
using UXI.Configuration;
using UXI.Configuration.Attributes;
using UXI.Configuration.Storages;

namespace UXC.Configuration
{
    [ConfigurationSection("App")]
    public class AppConfiguration : ConfigurationBase, IAppConfiguration
    {
        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield return StorageDefinition.Create("config.ini");
            }
        }

        public AppConfiguration(IConfigurationSource source) : base(source)
        {
            HideOnCloseProperty = CreateProperty(nameof(HideOnClose), DEFAULT_HideOnClose, source);
            // experimental
            ExperimentalProperty = CreateProperty(nameof(Experimental), DEFAULT_Experimental, source);
        }

        private const bool DEFAULT_HideOnClose = true;
        [global::System.Diagnostics.DebuggerNonUserCode]
        public bool HideOnClose
        {
            get { return HideOnCloseProperty.Get<bool>(); }
            set { HideOnCloseProperty.Set(value); }
        }

        private readonly ConfigurationSettingProperty HideOnCloseProperty;


        private const bool DEFAULT_Experimental = true;
        [global::System.Diagnostics.DebuggerNonUserCode]
        public bool Experimental
        {
            get { return ExperimentalProperty.Get<bool>(); }
            set { ExperimentalProperty.Set(value); }
        }

        private readonly ConfigurationSettingProperty ExperimentalProperty;
    }
}
