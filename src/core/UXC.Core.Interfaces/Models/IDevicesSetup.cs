using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;

namespace UXC.Models
{
    public interface IDevicesSetup
    {
        IEnumerable<DeviceType> Devices { get; }
        IDictionary<DeviceType, IDictionary<string, object>> Configurations { get; }
    }
}
