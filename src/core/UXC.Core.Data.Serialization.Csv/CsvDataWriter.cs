using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using UXI.Common;

namespace UXC.Core.Data.Serialization
{
    class CsvDataWriter : DisposableBase, IDataWriter, IDisposable
    {
        private readonly CsvWriter _writer;
        private readonly Type _dataType;
        private bool _isOpen = true;

        public CsvDataWriter(TextWriter writer, Type dataType, IEnumerable<ClassMap> maps)
        {
            _dataType = dataType; 

            _writer = new CsvWriter(writer);

            maps?.ForEach(map => _writer.Configuration.RegisterClassMap(map));

            _writer.WriteHeader(dataType);
            _writer.NextRecord();
        }


        public bool CanWrite(Type objectType)
        {
            return _isOpen && _dataType.Equals(objectType); 
        }


        public void Close()
        {
            if (_isOpen)
            {
                _isOpen = false;
                _writer.Flush();
            }
            _writer.Dispose();
        }


        public void Write(object data)
        {
            if (_isOpen)
            {
                _writer.WriteRecord(data);
            }
        }


        public void WriteRange(IEnumerable<object> data)
        {
            if (_isOpen)
            {
                _writer.WriteRecords(data);
            }
        }


        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    Close();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
