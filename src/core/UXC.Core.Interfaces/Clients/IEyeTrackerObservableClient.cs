using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Plugins.GazeAccess.Clients;
using UxLabClass.Adapters.Entities.Data;

namespace UXC.Core.Clients
{
    public interface IEyeTrackerObservableClient : IAdapterObservableClient<GazeData>
    {
    }
}
