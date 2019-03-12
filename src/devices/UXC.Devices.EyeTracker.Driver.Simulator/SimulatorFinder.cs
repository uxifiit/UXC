/**
 * UXC.Devices.EyeTracker.Driver.Simulator
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
using System.Threading;
using System.Threading.Tasks;

using UXI.SystemApi.Screen;
using UXI.SystemApi.Mouse;

namespace UXC.Devices.EyeTracker.Driver.Simulator
{
    public class SimulatorFinder : ITrackerFinder
    {
        public string Name => "Simulator";


        public Task<IEyeTrackerDriver> SearchAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IEyeTrackerDriver>(new SimulatorTracker(new ScreenParametersProvider(), new MouseCoordinatesHook()));
        }
    }
}
