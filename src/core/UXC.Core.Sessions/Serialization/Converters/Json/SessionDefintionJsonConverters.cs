using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UXC.Sessions.Timeline;

namespace UXC.Sessions.Serialization.Converters.Json
{
    public class SessionDefinitionJsonConverters : IEnumerable<JsonConverter>
    {
        public static IEnumerable<JsonConverter> Converters { get; } = new List<JsonConverter>(PointsJsonConverters.Converters)
        {
            new StringEnumConverter(camelCaseText: false),
            new DeviceTypeJsonConverter(),
            new TextJsonConverter(),
            new SingleOrArrayConverter<SessionStep>(),
            new WelcomeStepActionSettingsJsonConverter(),
            new SessionStepActionSettingsJsonConverter()
        };

        public IEnumerator<JsonConverter> GetEnumerator()
        {
            return Converters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
