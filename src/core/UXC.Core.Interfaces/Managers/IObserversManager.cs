using System;
using UXC.Observers;

namespace UXC.Core.Managers
{
    public interface IObserversManager : IManager<IDeviceObserver>, IDisposable
    {
    }
}
