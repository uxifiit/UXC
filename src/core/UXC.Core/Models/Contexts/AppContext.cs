/**
 * UXC.Core
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Helpers;

namespace UXC.Models.Contexts
{
    public class AppContext : IAppContext
    {
        public AppContext()
        {
            Version = Assembly.GetEntryAssembly().GetName().Version.ToString();

            HasAdminPrivileges = SystemHelper.IsAdministrator();
#if DEBUG
            IsInDebug = true;
#endif
        }

        public bool HasAdminPrivileges { get; }
        public bool IsInDebug { get; }
        public bool IsDesign { get { return false; } }
        public string Version { get; }
    }
}
