using System;
using UXC.Devices.Adapters;

namespace UXC.Core.Managers
{
    public interface IAdaptersManager : IManager<IDeviceAdapter>, IDisposable
    {
    }
}
