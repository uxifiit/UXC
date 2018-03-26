using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Core.Devices;
using UXC.Core.Modules;

namespace UXC.Core.Managers
{
    public class DevicesConnector : Connector<IDevice, IDeviceAdapter>
    {
        public DevicesConnector(IAdaptersManager manager, IEnumerable<IDevice> devices, IModulesService modules)
            : base(manager, devices, CreateAdapter, modules)
        {
        }

        private static IDeviceAdapter CreateAdapter(IDevice device)
        {
            return new DeviceAdapter(device);
        }
    }
}
