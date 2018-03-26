using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core;
using UXC.Core.Devices;
using UXC.Core.Data;

namespace UXC.Devices.Adapters
{
    public interface IObservableDevice
    {
        DeviceType DeviceType { get; }

        Type DataType { get; }

        IObservable<DeviceData> Data { get; }
        IObservable<DeviceStateChange> States { get; }
        IObservable<LogMessage> Logs { get; }
        IObservable<Exception> ConnectionErrors { get; } 

        // IObservable<Trigger> Control { get; }
    }
}
