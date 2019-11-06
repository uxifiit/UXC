/**
 * UXC.Core.Interfaces
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

namespace UXC.Core.Common
{
    /// <summary>
    /// Describes a class that is able to receive options of the specified type, e.g., from command line args.
    /// </summary>
    public interface IOptionsTarget
    {
        /// <summary>
        /// Gets the type of the options that are received by this class.
        /// </summary>
        Type OptionsType { get; }


        /// <summary>
        /// Receives the options of the type specified by the <seealso cref="OptionsType"/> property.
        /// </summary>
        /// <param name="options"></param>
        void ReceiveOptions(object options);
    }
}
