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
using UXC.Core.Data;
using UXI.Serialization.Formats.Json.Converters;
using UXI.Serialization.Formats.Json.Extensions;

namespace UXC.Core.Data.Serialization.Formats.Json.Converters
{
    public class GazeDataJsonConverter : GenericJsonConverter<GazeData>
    {
        protected override GazeData Convert(JToken token, JsonSerializer serializer)
        {
            var obj = (JObject)token;

            var deviceData = obj.ToObject<DeviceData>(serializer);

            var validity     = obj.GetValue<GazeDataValidity>(nameof(GazeData.Validity), serializer);
            var trackerTicks = obj.GetValue<long>(nameof(GazeData.TrackerTicks), serializer);
            var leftEye      = obj.GetValue<EyeGazeData>(nameof(GazeData.LeftEye), serializer);
            var rightEye     = obj.GetValue<EyeGazeData>(nameof(GazeData.RightEye), serializer);

            return new GazeData(validity, leftEye, rightEye, trackerTicks, deviceData.Timestamp);
        }
    }
}
