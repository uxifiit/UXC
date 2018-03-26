using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;

namespace UXC.Core.Models
{
    public struct DeviceStatus
    {
        public DeviceType DeviceType { get; }
        public DeviceState State { get; }
        public DateTime LastChange { get; set; }

        public DeviceStatus(DeviceType type, DeviceState state, DateTime lastChange)
        {
            DeviceType = type;
            State = state;
            LastChange = lastChange;
        }

        public DeviceStatus Update(DeviceState state, DateTime lastChange)
        {
            return new DeviceStatus(this.DeviceType, state, lastChange);
        }
    }

    public class DeviceStatusEventArgs : EventArgs
    {
        public DeviceStatus Status { get; }
        public DeviceStatusEventArgs(DeviceStatus status)
        {
            Status = status;
        }
    }
}
