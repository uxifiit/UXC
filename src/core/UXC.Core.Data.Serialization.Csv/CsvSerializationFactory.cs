using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace UXC.Core.Data.Serialization
{
    public class CsvSerializationFactory : IDataSerializationFactory
    {
        private readonly List<ClassMap> _maps;

        public CsvSerializationFactory(IEnumerable<ClassMap> maps)
        {
            _maps = maps?.ToList();
        }

        public string FileExtension => "csv";

        public string FormatName => "CSV";

        public string MimeType => "text/csv";

        public IDataReader CreateReaderForType(TextReader reader, Type dataType)
        {
            throw new NotImplementedException();
        }

        public IDataWriter CreateWriterForType(TextWriter writer, Type dataType)
        {
            return new CsvDataWriter(writer, dataType, _maps);
        }
    }
}
