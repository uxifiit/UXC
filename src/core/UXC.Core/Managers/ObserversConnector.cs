using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Observers;
using UXC.Core.Managers;
using UXC.Core.Modules;
using UXC.Managers;

namespace UXC
{
    public class ObserversConnector : Connector<IDeviceObserver>
    {
        public ObserversConnector(IObserversManager manager, IEnumerable<IDeviceObserver> clients, IModulesService modules)
            : base(manager, clients, modules)
        {
        }
    }
}
