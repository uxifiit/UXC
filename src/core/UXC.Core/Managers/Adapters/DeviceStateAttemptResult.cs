using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters.Commands;
using UXC.Core.Devices;
using UXC.Core.Models;

namespace UXC.Core.Managers.Adapters
{
    internal class DeviceStateAttemptResult
    {
        public DeviceStatus Status { get; }
        public CommandResult Result { get; }

        public DeviceStateAttemptResult(DeviceStatus status, CommandResult result)
        {
            Status = status;
            Result = result;
        }

        public DeviceStateAttemptResult(DeviceType type, DeviceState state, DateTime attemptAt, CommandResult result)
            : this(new DeviceStatus(type, state, attemptAt), result)
        {
        }
    }
}
