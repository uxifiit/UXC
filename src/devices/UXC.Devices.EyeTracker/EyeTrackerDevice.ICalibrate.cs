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
using UXC.Core.Devices.Calibration;
using UXC.Devices.EyeTracker.Calibration;

namespace UXC.Devices.EyeTracker
{
    public sealed partial class EyeTrackerDevice : ICalibrate
    {
        public ICalibrator GetCalibrator()
        {
            if (CanCalibrate)
            {
                var calibrator = new EyeTrackerCalibrator(this, _connectedTracker);
                CalibratorRequested?.Invoke(this, calibrator);
                return calibrator;
            }

            throw new InvalidOperationException("Not connected to any eye tracker.");
        }


        public event EventHandler<ICalibrator> CalibratorRequested;


        public bool IsCalibratorValid(ICalibrator calibrator)
        {
            if (calibrator is EyeTrackerCalibrator)
            {
                // TODO check if the calibrator uses the same tracker.
                return calibrator.State == CalibratorState.None;
            }
            return false;
        }


        public bool CanCalibrate
        {
            get { return _connectedTracker != null; }
        }

        public event EventHandler<bool> CanCalibrateChanged;

        private void OnCanCalibrateChanged()
        {
            CanCalibrateChanged?.Invoke(this, CanCalibrate);
        }
    }
}
