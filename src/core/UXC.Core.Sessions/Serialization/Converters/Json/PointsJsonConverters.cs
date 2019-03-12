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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UXC.Core.Data;
using UXC.Sessions.Common;

namespace UXC.Sessions.Serialization.Converters.Json
{
    public class PointsJsonConverters : IEnumerable<JsonConverter>
    {
        public static IEnumerable<JsonConverter> Converters { get; } = new JsonConverterCollection()
        {
            new Point2Converter(),
            new Point3Converter(),
        };


        public IEnumerator<JsonConverter> GetEnumerator()
        {
            return Converters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public class Point2Converter : JsonConverter<Point2>
    {
        protected override Point2 Convert(JToken token, JsonSerializer serializer)
        {
            double x, y;

            x = y = 0d;

            if (token.Type == JTokenType.Object)
            {
                var jObject = (JObject)token;
                x = jObject[nameof(Point2.X)].ToObject<double>(serializer);
                y = jObject[nameof(Point2.Y)].ToObject<double>(serializer);
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


    public class Point3Converter : JsonConverter<Point3>
    {
        protected override Point3 Convert(JToken token, JsonSerializer serializer)
        {
            double x, y, z;

            x = y = z = 0d;
           
            if (token.Type == JTokenType.Object)
            {
                var jObject = (JObject)token;
                x = jObject[nameof(Point3.X)].ToObject<double>(serializer);
                y = jObject[nameof(Point3.Y)].ToObject<double>(serializer);
                z = jObject[nameof(Point3.Z)].ToObject<double>(serializer);
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
