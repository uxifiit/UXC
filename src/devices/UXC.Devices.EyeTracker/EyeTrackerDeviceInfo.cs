using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;

namespace UXC.Devices.EyeTracker
{
    public class EyeTrackerDeviceInfo
    {
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public TrackBoxCoords TrackBox { get; set; }
        public string ProductId { get; set; }
    }
}
