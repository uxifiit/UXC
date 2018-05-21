using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UXC.Sessions.Common;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.Serialization.Converters.Json
{
    public class TextJsonConverter : JsonConverter<Text>
    {
        protected override Text Convert(JToken token, JsonSerializer serializer)
        {
            List<string> lines = null;

            if (token.Type == JTokenType.String)
            {
                lines = new List<string>() { token.Value<string>() };
            }
            else if (token.Type == JTokenType.Array)
            {
                var jArray = (JArray)token;
                lines = jArray.Select(e => e.Value<string>()).ToList();
            }
            else if (token.Type == JTokenType.Object) 
            {
                var jObject = (JObject)token;
                lines = jObject.GetValue(nameof(Text.Lines), StringComparison.CurrentCultureIgnoreCase).ToObject<List<string>>();
            }

            return new Text() { Lines = lines };
        }

        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Text text = value as Text;

            if (text != null)
            {
                writer.WriteStartArray();

                foreach (var line in text.Lines)
                {
                    writer.WriteValue(line);
                }

                writer.WriteEndArray();
            }
        }
    }
}
