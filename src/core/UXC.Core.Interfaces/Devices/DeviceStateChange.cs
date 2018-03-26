using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Devices
{
    public struct DeviceStateChange
    {
        public DeviceStateChange(DeviceState state, DateTime timestamp)
        {
            State = state;
            Timestamp = timestamp;
        }   

        public DeviceState State { get; }

        public DateTime Timestamp { get; }
    }
}
