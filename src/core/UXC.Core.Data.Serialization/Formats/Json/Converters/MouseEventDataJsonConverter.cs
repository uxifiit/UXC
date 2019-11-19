/**
 * UXC.Core.Data.Serialization
 * Copyright (c) 2019 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, COPYING.LESSER, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UXI.Serialization.Formats.Json.Converters;
using UXI.Serialization.Formats.Json.Extensions;

namespace UXC.Core.Data.Serialization.Formats.Json.Converters
{
    public class MouseEventDataJsonConverter : GenericJsonConverter<MouseEventData>
    {
        protected override MouseEventData Convert(JToken token, JsonSerializer serializer)
        {
            var obj = (JObject)token;

            var deviceData = obj.ToObject<DeviceData>(serializer);

            MouseEventType eventType = obj.GetValue<MouseEventType>(nameof(MouseEventData.EventType), serializer);
            MouseButton button = obj.GetValue<MouseButton>(nameof(MouseEventData.Button), serializer);
            int delta = obj.GetValue<int>(nameof(MouseEventData.Delta), serializer);
            int x = obj.GetValue<int>(nameof(MouseEventData.X), serializer);
            int y = obj.GetValue<int>(nameof(MouseEventData.Y), serializer);

            return new MouseEventData(eventType, x, y, button, delta, deviceData.Timestamp);
        }
    }
    
}
