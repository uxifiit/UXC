/**
 * UXC.Core.Interfaces
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Configuration;

namespace UXC.Core.Configuration
{
    /// <summary>
    /// Provides access the configuration of current object.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Gets the setting properties contained in the current <seealso cref="IConfiguration" />.
        /// </summary>
        IEnumerable<IConfigurationSettingProperty> Settings { get; }
    }
}
