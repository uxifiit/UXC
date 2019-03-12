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
using UXC.Models;

namespace UXC.Devices.EyeTracker
{
    /// <summary>
    /// Describes trackbox position in the User Coordinate System (UCS).
    /// The UCS is a millimeter-based system with its origin at the centre 
    /// of the frontal surface of the eye tracker (see Figure 1, Tobii Analytics SDK Developers Guide, p. 13).
    /// </summary>
    public class TrackBoxCoords
    { 
        public Point3 FrontUpperRight { get; set; }
        public Point3 FrontUpperLeft { get; set; }
        public Point3 FrontLowerLeft { get; set; }
        public Point3 FrontLowerRight { get; set; }
        public Point3 BackUpperRight { get; set; }
        public Point3 BackUpperLeft { get; set; }
        public Point3 BackLowerLeft { get; set; }
        public Point3 BackLowerRight { get; set; }
    }
}
