/**
 * UXC.Plugins.UXR
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
using UXC.Plugins.UXR.Models;
using UXI.Common.Extensions;
using UXR.Studies.Api.Entities.Nodes;

namespace UXC.Plugins.UXR.Extensions
{
    internal static class UXRNodeContextEx
    {
        public static void Update(this UXRNodeContext context, NodeIdInfo info)
        {
            context.ThrowIfNull(nameof(context));

            context.NodeId = info.Id;
          //  context.NodeName = info.Name;
            context.IsConnected = true;
        }
    }
}
