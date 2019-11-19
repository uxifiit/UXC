/**
 * UXC.Core.Data.Serialization
 * Copyright (c) 2019 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, COPYING.LESSER, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Serialization.Formats.Json.Converters;
using UXI.Serialization.Formats.Json.Extensions;

namespace UXC.Core.Data.Serialization.Formats.Json.Converters
{
    class KeyboardEventDataJsonConverter : GenericJsonConverter<KeyboardEventData>
    {
        protected override KeyboardEventData Convert(JToken token, JsonSerializer serializer)
        {
            var obj = (JObject)token;

            var deviceData = obj.ToObject<DeviceData>(serializer);

            KeyboardEventType eventType = obj.GetValue<KeyboardEventType>(nameof(KeyboardEventData.EventType), serializer);

            bool alt = obj.GetValue<bool>(nameof(KeyboardEventData.Alt), serializer);
            bool control = obj.GetValue<bool>(nameof(KeyboardEventData.Control), serializer);
            Key keyCode = obj.GetValue<Key>(nameof(KeyboardEventData.KeyCode), serializer);
            
            Key keyData = obj.GetValue<Key>(nameof(KeyboardEventData.KeyData), serializer);
            int keyValue = obj.GetValue<int>(nameof(KeyboardEventData.KeyValue), serializer);
            Key modifiers = obj.GetValue<Key>(nameof(KeyboardEventData.Modifiers), serializer);
            bool shift = obj.GetValue<bool>(nameof(KeyboardEventData.Shift), serializer);
            char keyChar = obj.GetValue<char>(nameof(KeyboardEventData.KeyChar), serializer);

            return new KeyboardEventData
            (
                eventType, 
                alt, 
                control, 
                keyCode, 
                keyData, 
                keyValue, 
                shift, 
                keyChar, 
                deviceData.Timestamp
            );
        }
    }
}
