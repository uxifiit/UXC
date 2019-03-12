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

namespace UXC.Core.Plugins
{
    public class Plugin 
    {
        public INinjectModule Module { get; internal set; }
        public string Assembly { get; internal set; }
        public Version Version { get; internal set; }
        public DateTime CreatedAt { get; internal set; }
    }
}
