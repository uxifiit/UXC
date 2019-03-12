/**
 * UXC.Plugins.GazeAccess
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

namespace UXC.Plugins.GazeAccess
{
    static class ApiRoutes
    {
        public const string PREFIX = "api/gaze";

        public static class GazeData
        {
            public const string PREFIX = ApiRoutes.PREFIX;

            // GET /api/gaze/
            public const string ACTION_RECENT = "";
        }
    }
}
