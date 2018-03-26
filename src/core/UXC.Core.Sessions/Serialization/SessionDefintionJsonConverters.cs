using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UXC.Core.Data.Serialization.Converters.Json;

namespace UXC.Sessions.Serialization
{
    class SessionDefinitionJsonConverters : IEnumerable<JsonConverter>
    {
        public static readonly IEnumerable<JsonConverter> Converters = new List<JsonConverter>(PointsJsonConverters.Converters)
        {
            new StringEnumConverter(camelCaseText: false),
            new DeviceTypeJsonConverter(),
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
