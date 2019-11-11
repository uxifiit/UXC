﻿/**
 * UXC.Core.Data.Serialization
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data.Serialization.Formats.Csv.Converters;
using UXI.Serialization.Formats.Csv.Configurations;
using UXI.Serialization.Formats.Csv.Converters;

namespace UXC.Core.Data.Serialization.Formats.Csv
{
    public class UXCDataCsvConvertersSerializationConfiguration : CsvConvertersSerializationConfiguration
    {
        public UXCDataCsvConvertersSerializationConfiguration()
            : base
            (
                new DeviceDataCsvConverter(),
                new EyeGazeDataCsvConverter(),
                new GazeDataCsvConverter(),
                new ExternalEventDataCsvConverter(),
                new KeyboardEventDataCsvConverter(),
                new MouseEventDataCsvConverter(),
                new Point2CsvConverter(),
                new Point3CsvConverter()
            )
        {
        }
    }
}