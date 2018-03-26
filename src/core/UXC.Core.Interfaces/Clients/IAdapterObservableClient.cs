using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UxLabClass.Adapters;
using UxLabClass.Adapters.Entities.Data;

namespace UXC.Plugins.GazeAccess.Clients
{
    public interface IAdapterObservableClient<T> where T : RecordingData
    {
        IObservable<T> Data { get; }
        IObservable<DeviceState> State { get; }
    }
}
