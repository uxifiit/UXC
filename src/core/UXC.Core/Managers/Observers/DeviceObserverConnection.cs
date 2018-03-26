using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXI.Common;
using UXC.Observers;
using UXI.Common.Extensions;

namespace UXC.Managers.Observers
{
    internal class DeviceObserverConnection : DisposableBase
    {
        public IDeviceAdapter Adapter { get; }
        private IDisposable _connection;
        public DeviceObserverConnection(IDeviceAdapter adapter, IDisposable connection)
        {
            Adapter = adapter;
            _connection = connection;
        }

        public void Disconnect()
        {
            var connection = ObjectEx.GetAndReplace(ref _connection, null);
            connection?.Dispose();
        }   

        #region IDisposable Members

        private bool _disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    Disconnect();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
