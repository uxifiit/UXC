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
using Newtonsoft.Json.Converters;
using UXC.Core.Data.Serialization.Formats.Json.Converters;
using UXI.Serialization.Formats.Json.Configurations;

namespace UXC.Core.Data.Serialization.Formats.Json
{
    public class UXCDataJsonConvertersSerializationConfiguration : JsonConvertersSerializationConfiguration
    {
        public UXCDataJsonConvertersSerializationConfiguration() 
            : base
            (
                new StringEnumConverter(camelCaseText: false),
                new DeviceDataJsonConverter(),
                new EyeGazeDataJsonConverter(),
                new GazeDataJsonConverter(),
                new ExternalEventDataJsonConverter(),
                new KeyboardEventDataJsonConverter(),
                new MouseEventDataJsonConverter(),
                new Point2JsonConverter(),
                new Point3JsonConverter()
            )
        {
        }
    }
}
