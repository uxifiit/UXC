using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Core.Devices;

namespace UXC.Devices.Adapters.Commands
{
    internal sealed class StopRecordingDeviceCommand : SyncDeviceCommand
    {
        internal StopRecordingDeviceCommand() 
            : base(DeviceAction.StopRecording, DeviceState.Connected) { }

        protected override CommandResult Execute(IDevice device)
        {
            return Result(device.StopRecording());
        }
    }
}
