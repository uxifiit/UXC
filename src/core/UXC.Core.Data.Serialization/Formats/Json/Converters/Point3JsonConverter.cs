/**
 * UXC.Core.Data.Serialization
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
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
    public class Point3JsonConverter : GenericJsonConverter<Point3>
    {
        protected override Point3 Convert(JToken token, JsonSerializer serializer)
        {
            double x, y, z;

            x = y = z = 0d;
           
            if (token.Type == JTokenType.Object)
            {
                var obj = (JObject)token;
                x = obj.GetValue<double>(nameof(Point3.X), serializer);
                y = obj.GetValue<double>(nameof(Point3.Y), serializer);
                z = obj.GetValue<double>(nameof(Point3.Z), serializer);
            }
            else if (token.Type == JTokenType.Array)
            {
                var array = (JArray)token;
                x = array[0].ToObject<double>(serializer);
                y = array[1].ToObject<double>(serializer);
                z = array[2].ToObject<double>(serializer);
            }


            return new Point3(x, y, z);
        }
    }
}
