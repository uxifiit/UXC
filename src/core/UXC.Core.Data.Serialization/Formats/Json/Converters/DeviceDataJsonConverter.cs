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
using UXI.Serialization.Formats.Json.Extensions;
using UXI.Serialization.Formats.Json.Converters;

namespace UXC.Core.Data.Serialization.Formats.Json.Converters
{
    public class DeviceDataJsonConverter : GenericJsonConverter<DeviceData> 
    {
        protected override DeviceData Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            DateTime timestamp = obj.GetValue<DateTime>(nameof(DeviceData.Timestamp), serializer);

            return new DeviceDataImpl(timestamp);
        }
    }
}
