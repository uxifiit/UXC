using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Models;
using UXC.Core.Devices;

namespace UXC.Devices.Adapters.Commands
{
    internal sealed class DisconnectDeviceCommand : SyncDeviceCommand
    {
        public DisconnectDeviceCommand() 
            : base(DeviceAction.Disconnect, DeviceState.Disconnected)
        { }

        protected override CommandResult Execute(IDevice device)
        {
            return Result(device.DisconnectDevice());
        }
    }
}
