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

namespace UXC.Core.Configuration
{
    /// <summary>
    /// IConfigurable Interface declares that the current object may be configurable using the supplied configuration through property.
    /// </summary>
    public interface IConfigurable
    {
        ///// <summary>
        ///// Gets the boolean value whether the device may be configured.
        ///// </summary>
        //bool CanConfigure { get; }

        /// <summary>
        /// Gets the currently effective configuration for the component.
        /// </summary>
        IConfiguration Configuration { get; }
    }
}
