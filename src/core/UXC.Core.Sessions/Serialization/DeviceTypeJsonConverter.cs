using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UXC.Core.Devices;

namespace UXC.Sessions.Serialization
{
    class DeviceTypeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(DeviceType).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string code = reader.Value as string;

            return DeviceType.GetOrCreate(code);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var device = (DeviceType)value;
            writer.WriteValue(device.Code);
        }

        public override bool CanRead => true;

        public override bool CanWrite => true;
    }
}
