/**
 * UXC.Plugins.SessionsAPI
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
using UXC.Core.Configuration;
using UXI.Configuration;

namespace UXC.Plugins.SessionsAPI.Recording.Configuration
{
    class BufferSessionRecorderConfiguration : IConfiguration
    {
        public BufferSessionRecorderConfiguration()
        {
            BufferSizeProperty = new ConfigurationSettingProperty(nameof(BufferSize), typeof(double), DEFAULT_BufferSize);
        }


        public IEnumerable<IConfigurationSettingProperty> Settings
        {
            get
            {
                yield return BufferSizeProperty;
            }
        }


        private static readonly double DEFAULT_BufferSize = 1000d;
        [global::System.Configuration.Setting]
        public double BufferSize
        {
            get
            {
                return BufferSizeProperty.Get<double>();
            }
        }
        private readonly ConfigurationSettingProperty BufferSizeProperty;
    }
}
