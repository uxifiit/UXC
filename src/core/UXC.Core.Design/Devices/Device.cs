using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Devices.Adapters;

namespace UXC.Design.Devices
{
    class DesignDevice : IDevice
    {
        public DesignDevice(DeviceType deviceType)
        {
            Code = DeviceCode.Create(this, deviceType.Code).Build(); 
        }

        public DeviceCode Code { get; }

        public Type DataType { get; } = typeof(DeviceData);

        public event ErrorOccurredEventHandler ConnectionError { add { } remove { } } 
        public event DeviceDataReceivedEventHandler Data { add { } remove { } }
        public event DeviceLogEventHandler Log { add { } remove { } }

        public bool ConnectToDevice()
        {
            return true;
        }

        public bool DisconnectDevice()
        {
            return true;
        }

        public void DumpInfo()
        {
        }

        public bool StartRecording()
        {
            return true;
        }

        public bool StopRecording()
        {
            return true;
        }
    }
}
