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
    public class WelcomeStepActionSettingsJsonConverter : JsonConverter<WelcomeActionSettings>
    {
        protected override WelcomeActionSettings Convert(JToken token, JsonSerializer serializer)
        {
            WelcomeActionSettings welcome = new WelcomeActionSettings();

            // Populate the object properties
            serializer.Populate(token.CreateReader(), welcome);

            return welcome;
        }
    }
}
