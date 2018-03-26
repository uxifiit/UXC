using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Core.Devices;

namespace UXC.Models
{
    public class DevicesSetup : IDevicesSetup
    {
        public DevicesSetup(IEnumerable<DeviceType> devices) 
        {
        }

        public DevicesSetup(IEnumerable<DeviceType> devices, IDictionary<DeviceType, IDictionary<string, object>> configurations)
        {
            Devices = devices?.ToArray() ?? Enumerable.Empty<DeviceType>();
            Configurations = configurations;            
        }

        public DevicesSetup(IDevicesSetup setup)
            : this(setup.Devices, setup.Configurations)
        {   
            
        }

        public IEnumerable<DeviceType> Devices
        {
            get;
        }

        public IDictionary<DeviceType, IDictionary<string, object>> Configurations
        {
            get;
        }
    }
}
