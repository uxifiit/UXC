using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UXC.Devices.EyeTracker.TobiiPro
{
    public class TobiiProFinder : ITrackerFinder
    {
        public string Name => "Tobii Pro";

        public async Task<IEyeTrackerDriver> SearchAsync(CancellationToken cancellationToken)
        {
            var trackers = await Tobii.Research.EyeTrackingOperations.FindAllEyeTrackersAsync();

            if (trackers.Any())
            {
                return new TobiiProTracker(trackers.First());
            }

            return null;
        }
    }
}
