/**
 * UXC.Devices.EyeTracker.Calibration.UI
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System.Collections.Generic;
using UXC.Devices.EyeTracker.Calibration;

namespace UXC.Devices.EyeTracker.Models
{
    public interface ICalibrationProfilesService
    {
        IEnumerable<CalibrationInfo> GetStoredCalibrations();
        IEnumerable<CalibrationInfo> GetStoredCalibrations(string deviceFamilyName);
        CalibrationData LoadCalibration(CalibrationInfo calibration);
        bool TrySaveCalibration(string name, CalibrationData calibration, out CalibrationInfo info);
    }
}
