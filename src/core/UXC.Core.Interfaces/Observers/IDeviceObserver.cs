using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Core.Devices;

namespace UXC.Observers
{
    public interface IDeviceObserver // TODO rename
    {
        /// <summary>
        /// Checks whether the given type of device is supported by the current <seealso cref="IDeviceObserver" />.
        /// </summary>
        /// <param name="type">Type of device to check.</param>
        /// <returns>true if is supported, otherwise false.</returns>
        bool IsDeviceSupported(DeviceType type);

        /// <summary>
        /// Connects an observer to the given device and returns instance of IDisposable for disconnecting the client.
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        IDisposable Connect(IObservableDevice device);
    }
}
