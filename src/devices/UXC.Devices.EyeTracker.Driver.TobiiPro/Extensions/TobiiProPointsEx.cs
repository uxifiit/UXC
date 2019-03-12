/**
 * UXC.Devices.EyeTracker.Driver.TobiiPro
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

using UXC.Devices.EyeTracker.Calibration;
using UXC.Core.Data;

namespace UXC.Devices.EyeTracker.TobiiPro
{
    internal static class TobiiProPointsEx
    {
        public static Point3 ToPoint3(this Tobii.Research.Point3D point)
        {
            return new Point3(point.X, point.Y, point.Z);
        }


        public static Point2 ToPoint2(this Tobii.Research.NormalizedPoint2D point)
        {
            return new Point2(point.X, point.Y);
        }


        public static Point3 ToPoint2(this Tobii.Research.NormalizedPoint3D point)
        {
            return new Point3(point.X, point.Y, point.Z);
        }


        public static Tobii.Research.NormalizedPoint2D ToNormalizedPoint2D(this Point2 point)
        {
            return new Tobii.Research.NormalizedPoint2D(Convert.ToSingle(point.X), Convert.ToSingle(point.Y));
        }
    }
}
