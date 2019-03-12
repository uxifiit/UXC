/**
 * UXC.Devices.EyeTracker
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

namespace UXC.Devices.EyeTracker.Configuration
{
    [ConfigurationSection("Devices.EyeTracker")]
    public class EyeTrackerConfiguration : ConfigurationBase, IEyeTrackerConfiguration
    {
        public EyeTrackerConfiguration(IConfigurationSource source) : base(source)
        {
            SelectedDriverProperty = CreateProperty(nameof(SelectedDriver), DEFAULT_SelectedDriver, source);
        }


        private const string DEFAULT_SelectedDriver = "Tobii Pro";
        [global::System.Diagnostics.DebuggerNonUserCode]
        public string SelectedDriver
        {
            get { return SelectedDriverProperty.Get<string>(); }
            set { SelectedDriverProperty.Set(value); }
        }

        private readonly ConfigurationSettingProperty SelectedDriverProperty;
    }
}
