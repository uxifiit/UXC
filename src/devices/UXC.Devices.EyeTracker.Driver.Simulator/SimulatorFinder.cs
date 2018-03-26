using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UXI.SystemApi.Screen;
using UXI.SystemApi.Mouse;

namespace UXC.Devices.EyeTracker.Simulator
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
