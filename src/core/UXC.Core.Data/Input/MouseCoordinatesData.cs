/**
 * UXC.Core.Data
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

namespace UXC.Core.Data
{
    public class MouseCoordinatesData : DeviceData
    {
        public int X { get; }
        public int Y { get; }

        public double NormalizedX { get; }
        public double NormalizedY { get; }


        public MouseCoordinatesData(int x, int y, double normx, double normy)
            : this(x, y, normx, normy, DateTime.Now)
        {
           
        }

        public MouseCoordinatesData(int x, int y, double normx, double normy, DateTime timestamp)
            : base(timestamp)
        {
            X = x;
            Y = y;
            NormalizedX = normx;
            NormalizedY = normy;
        }
    }
}
