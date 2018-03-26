using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXC.Devices.Adapters;

namespace UXC.Observers
{
    public class DeviceLogger : IDeviceObserver
    {
        public IDisposable Connect(IObservableDevice device)
        {

            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            throw new NotImplementedException();
        }

        public bool IsDeviceSupported(DeviceType type)
        {
            return true;
        }
    }
}
