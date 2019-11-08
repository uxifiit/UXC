/**
 * UXC.Core.Data.Serialization
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UXC.Core.Data;

namespace UXC.Core.Data.Serialization.Formats.Json.Converters
{
    public class DataJsonConverters : IEnumerable<JsonConverter>
    {
        public static IEnumerable<JsonConverter> Converters { get; } = new List<JsonConverter>(PointsJsonConverters.Converters)
        {
            new StringEnumConverter(camelCaseText: false),
            new EyeGazeDataJsonConverter(),
            new GazeDataJsonConverter(),
            new ExternalEventDataJsonConverter()
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
}
