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

namespace UXC.Configuration.Design
{
    class DesignAppConfiguration : IAppConfiguration
    {
        public bool Experimental
        {
            get { return false; } set { }
        }

        public bool HideOnClose
        {
            get { return true; } set { }
        }
    }
}
