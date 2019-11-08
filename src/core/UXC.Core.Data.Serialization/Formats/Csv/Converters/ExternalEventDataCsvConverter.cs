/**
 * UXC.Core.Data.Serialization
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
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
    public class ExternalEventDataCsvConverter : CsvConverter<ExternalEventData>
    {
        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<DeviceData>(writer, naming);

            writer.WriteField(naming.Get(nameof(ExternalEventData.Source)));
            writer.WriteField(naming.Get(nameof(ExternalEventData.EventName)));
            writer.WriteField(naming.Get(nameof(ExternalEventData.EventData)));
            writer.WriteField(naming.Get(nameof(ExternalEventData.ValidTill)));
        }


        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref ExternalEventData result)
        {
            DeviceData deviceData;

            string source;
            string eventName;
            string eventData;
            DateTime? validTill;

            if (

                    TryGetMember<DeviceData>(reader, serializer, naming, out deviceData)
                 && reader.TryGetField<string>(naming.Get(nameof(ExternalEventData.Source)), out source)
                 && reader.TryGetField<string>(naming.Get(nameof(ExternalEventData.EventName)), out eventName)
                 && reader.TryGetField<string>(naming.Get(nameof(ExternalEventData.EventData)), out eventData)
                 && reader.TryGetField<DateTime?>(naming.Get(nameof(ExternalEventData.ValidTill)), out validTill)
               )
            {
                result = new ExternalEventData
                (
                    source,
                    eventName,
                    eventData,
                    deviceData.Timestamp,
                    validTill
                );

                return true;
            }

            return false;
        }


        protected override void Write(ExternalEventData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize<DeviceData>(writer, data);

            writer.WriteField(data.Source);
            writer.WriteField(data.EventName);
            writer.WriteField(data.EventData);
            writer.WriteField(data.ValidTill);
        }
    }
}
