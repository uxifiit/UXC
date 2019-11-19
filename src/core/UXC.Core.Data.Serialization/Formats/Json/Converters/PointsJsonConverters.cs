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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UXC.Core.Data.Serialization.Formats.Json.Converters
{
    public class PointsJsonConverters : IEnumerable<JsonConverter>
    {
        public static IEnumerable<JsonConverter> Converters { get; } = new JsonConverterCollection()
        {
            new Point2JsonConverter(),
            new Point3JsonConverter(),
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
