/**
 * UXC.Core.Data.Serialization
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UXI.Serialization.Formats.Json.Extensions;
using UXI.Serialization.Formats.Json.Converters;

namespace UXC.Core.Data.Serialization.Formats.Json.Converters
{
    public class ExternalEventDataJsonConverter : GenericJsonConverter<ExternalEventData>
    {
        protected override ExternalEventData Convert(JToken token, JsonSerializer serializer)
        {
            var obj = (JObject)token;

            var deviceData = obj.ToObject<DeviceData>(serializer);

            string source       = obj.GetValue<string>(nameof(ExternalEventData.Source), serializer);
            string eventName    = obj.GetValue<string>(nameof(ExternalEventData.EventName), serializer);
            string eventData    = obj.GetValue<string>(nameof(ExternalEventData.EventData), serializer);
            DateTime? validTill = obj.GetValue<DateTime?>(nameof(ExternalEventData.ValidTill), serializer);

            return new ExternalEventData(source, eventName, eventData, deviceData.Timestamp, validTill);
        }
    }
}
