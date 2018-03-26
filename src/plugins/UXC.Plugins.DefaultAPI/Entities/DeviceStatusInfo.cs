using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;

namespace UXC.Plugins.DefaultAPI.Entities
{
    public class DeviceStatusInfo
    {
        public string Device { get; set; }
        public DeviceState State { get; set; } 
        public DateTime LastChange { get; set; }
    }
}
