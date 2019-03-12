/**
 * UXC.Core.Interfaces
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
using UXI.Common;
using UXC.Core.Common.Events;

namespace UXC.Core.Devices.Calibration
{
    /// <summary>
    /// ICalibrator defines basic common interface for calibrators, i.e., objects that perform calibration of a device. Consumer should cast the ICalibrator instance to the device specific calibrator implementation for further actions.
    /// </summary>
    public interface ICalibrator
    {
        /// <summary>
        /// Gets the target device which is calibrated by the current <seealso cref="ICalibrator" /> instance.
        /// </summary>
        ICalibrate Target { get; }

        /// <summary>
        /// Gets the current state of the calibrator.
        /// </summary>
        CalibratorState State { get; }


        event CalibratorStateChangedEventHandler StateChanged;


        Task CancelAsync();
    }
}
