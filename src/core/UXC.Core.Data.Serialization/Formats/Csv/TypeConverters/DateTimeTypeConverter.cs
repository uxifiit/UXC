using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Data.Serialization.Formats.Csv.TypeConverters
{
    class DateTimeTypeConverter : ITypeConverter
    {
        public DateTimeTypeConverter()
        {
        }


        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return DateTime.Parse(text);
        }


        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return ((DateTime)value).ToString("o");
        }
    }
}
