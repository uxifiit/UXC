using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Core.Data.Serialization;

namespace UXC.Plugins.SessionsAPI.Recording
{
    class InMemoryDataBufferWriter : IDataWriter
    {
        private readonly DeviceType _device;
        private readonly InMemoryRecordingDataAccessBuffer _buffer;
        private readonly Type _dataType;
        private bool _isOpen;

        public InMemoryDataBufferWriter(DeviceType device, InMemoryRecordingDataAccessBuffer buffer, Type dataType)
        {
            _device = device;
            _buffer = buffer;
            _dataType = dataType;
            _isOpen = true;
        }


        public bool CanWrite(Type objectType)
        {
            return _dataType.Equals(objectType);
        }


        public void Close()
        {
            _isOpen = false;
        }


        public void Dispose()
        {
            Close();
        }


        public void Write(object data)
        {
            if (_isOpen)
            {
                _buffer.Add(_device, (DeviceData)data);
            }
        }


        public void WriteRange(IEnumerable<object> data)
        {
            if (_isOpen && data != null && data.Any())
            {
                _buffer.Add(_device, new DeviceDataPart(data.OfType<DeviceData>()));
            }
        }
    }
}
