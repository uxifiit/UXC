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

namespace UXC.Core
{
    [Flags]
    public enum AppState
    {
        None = 0,
        Working = 1,

        Loading = None | Working,
        Loaded = 2,

        Starting = Loaded | Working,
        Started = Loaded | 4,

        Error = 16
    }
}
