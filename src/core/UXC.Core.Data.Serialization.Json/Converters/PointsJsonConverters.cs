using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UXC.Core.Data.Serialization.Converters.Json
{
    public static class PointsJsonConverters
    {
        public static readonly IEnumerable<JsonConverter> Converters = new JsonConverterCollection()
        {
            new Point2Converter(),
            new Point3Converter(),
        };
    }


    class Point2Converter : JsonConverter<Point2>
    {
        protected override Point2 Convert(JToken token, JsonSerializer serializer)
        {
            double x, y;

            x = y = 0d;

            if (token.Type == JTokenType.Object)
            {
                var jObject = (JObject)token;
                x = jObject[nameof(Point2.X)].ToObject<double>(serializer);
                y = jObject[nameof(Point2.Y)].ToObject<double>(serializer);
            }
            else if (token.Type == JTokenType.Array)
            {
                var array = (JArray)token;
                x = array[0].ToObject<double>(serializer);
                y = array[1].ToObject<double>(serializer);
            }

            return new Point2(x, y);
        }
    }


    class Point3Converter : JsonConverter<Point3>
    {
        protected override Point3 Convert(JToken token, JsonSerializer serializer)
        {
            double x, y, z;

            x = y = z = 0d;
           
            if (token.Type == JTokenType.Object)
            {
                var jObject = (JObject)token;
                x = jObject[nameof(Point3.X)].ToObject<double>(serializer);
                y = jObject[nameof(Point3.Y)].ToObject<double>(serializer);
                z = jObject[nameof(Point3.Z)].ToObject<double>(serializer);
            }
            else if (token.Type == JTokenType.Array)
            {
                var array = (JArray)token;
                x = array[0].ToObject<double>(serializer);
                y = array[1].ToObject<double>(serializer);
                z = array[2].ToObject<double>(serializer);
            }


            return new Point3(x, y, z);
        }
    }
}
