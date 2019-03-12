/**
 * UXC.Devices.ExternalEvents
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

namespace UXC.Devices.ExternalEvents
{
    static class ApiRoutes
    {
        public const string PREFIX = "api/event";

        public static class ExternalEvents
        {
            public const string PREFIX = ApiRoutes.PREFIX;

            public const string ACTION_SEND = "";

            public const string ACTION_SEND_MANY = "many";
        }
    }
}
