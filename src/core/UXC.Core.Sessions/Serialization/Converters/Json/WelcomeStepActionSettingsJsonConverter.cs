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
using UXI.Serialization.Formats.Json.Converters;

namespace UXC.Sessions.Serialization.Converters.Json
{
    public class WelcomeStepActionSettingsJsonConverter : GenericJsonConverter<WelcomeActionSettings>
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
