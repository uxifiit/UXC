using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Devices.EyeTracker.Configuration
{
    public interface IEyeTrackerConfiguration
    {
        string SelectedDriver { get; set; }
    }
}
