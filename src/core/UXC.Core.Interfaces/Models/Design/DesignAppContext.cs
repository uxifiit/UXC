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

namespace UXC.Models.Contexts.Design
{
    public class DesignAppContext : IAppContext
    {
        public bool HasAdminPrivileges => false;

        public bool IsInDebug => true;

        public string Version => new Version(0, 1, 1).ToString();

        public bool IsDesign => true;
    }
}
