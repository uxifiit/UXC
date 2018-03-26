using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters.Commands;
using UXC.Core.Devices;
using System.Collections.ObjectModel;

namespace UXC.Devices.Adapters.Commands
{
    public class DeviceCommands : IEnumerable<IDeviceCommand>
    {
        internal DeviceCommands(DeviceAdapter adapter)
        {
            Forward = new ReadOnlyCollection<IDeviceCommand>(new List<IDeviceCommand>()
            {
                new ConnectDeviceCommand(),
                new StartRecordingDeviceCommand()
            });
        
            Backward = new ReadOnlyCollection<IDeviceCommand>(new List<IDeviceCommand>()
            {
                new StopRecordingDeviceCommand(),
                new DisconnectDeviceCommand()
            });
        }

        public IEnumerable<IDeviceCommand> Forward { get; }
        public IEnumerable<IDeviceCommand> Backward { get; }



        #region IEnumerable<IDeviceCommand> Members

        public IEnumerator<IDeviceCommand> GetEnumerator()
        {
            return Forward.Concat(Backward).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
