using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UXC.Plugins.SessionsAPI.Entities
{
    //internal class SessionDeviceDefinitionInfoJsonConverter : JsonConverter
    //{
    //    public override bool CanWrite => false;

    //    public override bool CanConvert(Type objectType)
    //    {
    //        return objectType.Equals(typeof(SessionDeviceDefinitionInfo));
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        var token = JToken.Load(reader);
    //        if (token.Type == JTokenType.String)
    //        {
    //            return new SessionDeviceDefinitionInfo() { Code = token.ToObject<string>() };
    //        }
    //        else
    //        {
    //            return token.ToObject<SessionDeviceDefinitionInfo>();
    //        }
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    /// <summary>
    ///     Converts single JSON entries into lists
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SingleOrArrayConverter<T> : JsonConverter
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                return token.ToObject<List<T>>();
            }
            return new List<T> { token.ToObject<T>() };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
