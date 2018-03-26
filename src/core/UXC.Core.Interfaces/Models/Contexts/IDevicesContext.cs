using System;
using System.Collections.Generic;
using UXC.Core.Models;

namespace UXC.Models.Contexts
{
    public interface IDevicesContext 
    {
        IEnumerable<DeviceStatus> Devices { get; }

        event EventHandler<DeviceStatus> DeviceUpdated;
        event EventHandler<DeviceStatus> DeviceAdded;
        event EventHandler<DeviceStatus> DeviceRemoved;
    }
}
