using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Core.Devices;

namespace UXC.Core.Managers.Adapters
{
    public class UserDeviceActionRequest
    {
        public UserDeviceActionRequest(DeviceType device, DeviceState state)
        {
            Device = device;
            TargetState = state;
        }   
        public DeviceType Device { get; private set; }
        public DeviceState TargetState { get; private set; }
    }

    public class UserDeviceActionRequestedEventArgs
    {
        public IEnumerable<UserDeviceActionRequest> Requests { get; private set; }

        public UserDeviceActionRequestedEventArgs(IEnumerable<UserDeviceActionRequest> requests)
        {
            Requests = requests.ToArray();
        }
    }
}
