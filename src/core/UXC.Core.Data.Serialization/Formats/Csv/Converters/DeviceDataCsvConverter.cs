/**
 * UXC.Core.Data.Serialization
 * Copyright (c) 2019 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, COPYING.LESSER, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Serialization.Formats.Csv;
using UXI.Serialization.Formats.Csv.Converters;

namespace UXC.Core.Data.Serialization.Formats.Csv.Converters
{
    public class DeviceDataCsvConverter : CsvConverter<DeviceData>
    {
        public override bool CanConvert(Type objectType)
        {
            return base.CanConvert(objectType) || objectType == typeof(DeviceDataImpl);
        }


        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref DeviceData result)
        {
            DateTime timestamp;

            if (reader.TryGetField<DateTime>(naming.Get(nameof(DeviceData.Timestamp)), out timestamp))
            {
                result = new DeviceDataImpl(timestamp);

                return true;
            }
            
            return false;
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(DeviceData.Timestamp)));
        }


        protected override void Write(DeviceData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Timestamp);
        }
    }
}
