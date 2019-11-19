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
using UXC.Core.Data;
using UXI.Serialization.Formats.Json.Converters;
using UXI.Serialization.Formats.Json.Extensions;

namespace UXC.Core.Data.Serialization.Formats.Json.Converters
{
    public class Point2JsonConverter : GenericJsonConverter<Point2>
    {
        protected override Point2 Convert(JToken token, JsonSerializer serializer)
        {
            double x, y;

            x = y = 0d;

            if (token.Type == JTokenType.Object)
            {
                var obj = (JObject)token;
                x = obj.GetValue<double>(nameof(Point2.X), serializer);
                y = obj.GetValue<double>(nameof(Point2.Y), serializer);
            }
            else if (token.Type == JTokenType.Array)
            {
                var array = (JArray)token;
                x = array[0].ToObject<double>(serializer);
                y = array[1].ToObject<double>(serializer);
            }

            return new Point2(x, y);
        }
    }
}
