using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Core.Devices;

namespace UXC.Devices.Adapters.Commands
{
    internal sealed class ConnectDeviceCommand : SyncDeviceCommand
    {
        internal ConnectDeviceCommand()
            : base(DeviceAction.Connect, DeviceState.Connected)
        { }

        protected override CommandResult Execute(IDevice device)
        {
            return Result(device.ConnectToDevice());
        }
    }
}
