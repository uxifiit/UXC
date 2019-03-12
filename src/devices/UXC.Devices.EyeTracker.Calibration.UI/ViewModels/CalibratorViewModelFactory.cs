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
using UXI.Common.Extensions;
using UXC.Core.ViewModels;
using UXC.Core.ViewServices;
using UXC.Devices.EyeTracker.Calibration;
using UXC.Devices.EyeTracker.Models;

namespace UXC.Devices.EyeTracker.ViewModels
{
    public class CalibratorViewModelFactory : ViewModelFactory<EyeTrackerCalibrator, CalibratorViewModel>
    {
        private readonly ICalibrationProfilesService _calibrations;

        public CalibratorViewModelFactory(ICalibrationProfilesService calibrations)
        {
            _calibrations = calibrations;
        }

        protected override CalibratorViewModel CreateInternal(EyeTrackerCalibrator source)
        {
            source.ThrowIfNull(nameof(source));
            source.ThrowIf(s => s.GetType().IsInstanceOfType(SourceType), nameof(source), $"The type does not match the ${nameof(SourceType)} = ${SourceType.FullName}");

            return new CalibratorViewModel(source, _calibrations);
        }
    }
}
