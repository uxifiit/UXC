using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UXC.Devices.EyeTracker
{
    public interface ITrackerFinder
    {
        string Name { get; }

        Task<IEyeTrackerDriver> SearchAsync(CancellationToken token);
    }
}
