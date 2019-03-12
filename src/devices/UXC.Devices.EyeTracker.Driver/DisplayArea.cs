/**
 * UXC.Devices.EyeTracker.Driver
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
using UXC.Core.Data;

namespace UXC.Devices.EyeTracker
{
    public class DisplayArea
    {
        public DisplayArea(Point3 bottomLeft, Point3 topLeft, Point3 topRight)
        {
            BottomLeft = bottomLeft;
            TopLeft = topLeft;
            TopRight = topRight;
        }

        public Point3 BottomLeft { get; }
        public Point3 TopLeft { get; }
        public Point3 TopRight { get; }
    }
}
