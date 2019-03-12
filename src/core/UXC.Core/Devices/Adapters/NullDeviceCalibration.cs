/**
 * UXC.Core
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
using UXC.Core.Devices.Calibration;

namespace UXC.Devices.Adapters
{
    class NullDeviceCalibration : ICalibrate
    {
        private static readonly Lazy<NullDeviceCalibration> instance = new Lazy<NullDeviceCalibration>();
        public static NullDeviceCalibration Instance => instance.Value;


        public event EventHandler<ICalibrator> CalibratorRequested { add { } remove { } }


        public ICalibrator GetCalibrator()
        {
            throw new NotSupportedException();
        }

      
        public bool IsCalibratorValid(ICalibrator calibrator) => false;


        public bool CanCalibrate => false;


        public event EventHandler<bool> CanCalibrateChanged { add { } remove { } }
    }
}
