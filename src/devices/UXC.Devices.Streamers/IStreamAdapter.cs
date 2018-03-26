using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;

namespace UXC.Devices.Streamers
{
    public interface IStreamAdapter : IDeviceAdapter
    {
        IEnumerable<string> GetCompatibleDevices();
    }
}
