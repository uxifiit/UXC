/**
 * UXC.Core.Sessions
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
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
    public class SessionStepActionSettingsJsonConverter : InheritedObjectJsonConverter<SessionStepActionSettings>
    {
        protected override Type ResolveType(JObject jObject)
        {
            JToken settingsTypeToken;
            Type type;
            if (jObject.TryGetValue(nameof(SessionStepActionSettings.ActionType), StringComparison.InvariantCultureIgnoreCase, out settingsTypeToken)
                && SessionStepActionSettings.TryResolve(settingsTypeToken.ToObject<string>(), out type))
            {
                return type;
            }
            throw new ArgumentOutOfRangeException(nameof(SessionStepActionSettings.ActionType), "Failed to identify action type from the JSON object. ActionType field is either missing or contains unsupported value.");                       
        }


        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
