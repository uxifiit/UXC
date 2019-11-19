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
    public class MouseEventDataCsvConverter : CsvConverter<MouseEventData>
    {
        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<DeviceData>(writer, naming);

            writer.WriteField(naming.Get(nameof(MouseEventData.EventType)));
            writer.WriteField(naming.Get(nameof(MouseEventData.X)));
            writer.WriteField(naming.Get(nameof(MouseEventData.Y)));
            writer.WriteField(naming.Get(nameof(MouseEventData.Button)));
            writer.WriteField(naming.Get(nameof(MouseEventData.Delta)));
        }


        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref MouseEventData result)
        {
            DeviceData deviceData;

            MouseEventType eventType;
            int x;
            int y;
            MouseButton button;
            int delta;


            if (
                    TryGetMember<DeviceData>(reader, serializer, naming, out deviceData)
                 && reader.TryGetField<MouseEventType>(naming.Get(nameof(MouseEventData.EventType)), out eventType)
                 && reader.TryGetField<int>(naming.Get(nameof(MouseEventData.X)), out x)
                 && reader.TryGetField<int>(naming.Get(nameof(MouseEventData.Y)), out y)
                 && reader.TryGetField<MouseButton>(naming.Get(nameof(MouseEventData.Button)), out button)
                 && reader.TryGetField<int>(naming.Get(nameof(MouseEventData.Delta)), out delta)
               )
            {
                result = new MouseEventData(eventType, x, y, button, delta, deviceData.Timestamp);

                return true;
            }

            return false;
        }


        protected override void Write(MouseEventData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize<DeviceData>(writer, data);

            writer.WriteField(data.EventType);
            writer.WriteField(data.X);
            writer.WriteField(data.Y);
            writer.WriteField(data.Button);
            writer.WriteField(data.Delta);
        }
    }
}
