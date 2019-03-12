/**
 * UXC.Devices.EyeTracker
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

namespace UXC.Devices.EyeTracker.Calibration
{
    public class CalibrationExecutionReport
    {
        internal CalibrationExecutionReport(CalibrationPlan plan, CalibrationResult result, CalibrationData data)
        {
            Plan = plan;
            Result = result;
            Data = data;
        }


        public CalibrationPlan Plan { get; }


        public CalibrationResult Result { get; }


        public CalibrationData Data { get; }
    }
}
