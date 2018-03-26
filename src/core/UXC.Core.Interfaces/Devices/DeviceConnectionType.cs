using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Devices
{
    public enum DeviceConnectionType : int
    {
        None = 0,
        SystemApi = 1,
        Process = 2,
        Port = 3
    }
}
