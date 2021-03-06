/**
 * UXC.Devices.EyeTracker.Calibration.UI
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

namespace UXC.Devices.EyeTracker
{
    internal static class ExponentialSmoothing
    {
        public static double Smooth(double previous, double current, double alpha)
        {
            return previous * (1 - alpha) + current * alpha;
        }
    }
}
