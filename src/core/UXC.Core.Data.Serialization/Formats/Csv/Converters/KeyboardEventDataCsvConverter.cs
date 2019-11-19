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
    public class KeyboardEventDataCsvConverter : CsvConverter<KeyboardEventData>
    {
        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<DeviceData>(writer, naming);

            writer.WriteField(naming.Get(nameof(KeyboardEventData.EventType)));
            writer.WriteField(naming.Get(nameof(KeyboardEventData.Alt)));
            writer.WriteField(naming.Get(nameof(KeyboardEventData.Control)));
            writer.WriteField(naming.Get(nameof(KeyboardEventData.Shift)));
            writer.WriteField(naming.Get(nameof(KeyboardEventData.KeyCode)));
            writer.WriteField(naming.Get(nameof(KeyboardEventData.KeyData)));
            writer.WriteField(naming.Get(nameof(KeyboardEventData.KeyValue)));
            writer.WriteField(naming.Get(nameof(KeyboardEventData.Modifiers)));
            writer.WriteField(naming.Get(nameof(KeyboardEventData.KeyChar)));
        }


        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref KeyboardEventData result)
        {
            DeviceData deviceData;

            KeyboardEventType eventType;
            bool alt;
            bool control;
            Key keyCode;
            Key keyData;
            int keyValue;
            Key modifiers;
            bool shift;
            char keyChar;

            if (
                    TryGetMember<DeviceData>(reader, serializer, naming, out deviceData)
                 && reader.TryGetField<KeyboardEventType>(naming.Get(nameof(KeyboardEventData.EventType)), out eventType)
                 && reader.TryGetField<bool>(naming.Get(nameof(KeyboardEventData.Alt)), out alt)
                 && reader.TryGetField<bool>(naming.Get(nameof(KeyboardEventData.Control)), out control)
                 && reader.TryGetField<Key>(naming.Get(nameof(KeyboardEventData.KeyCode)), out keyCode)
                 && reader.TryGetField<Key>(naming.Get(nameof(KeyboardEventData.KeyData)), out keyData)
                 && reader.TryGetField<int>(naming.Get(nameof(KeyboardEventData.KeyValue)), out keyValue)
                 && reader.TryGetField<Key>(naming.Get(nameof(KeyboardEventData.Modifiers)), out modifiers)
                 && reader.TryGetField<bool>(naming.Get(nameof(KeyboardEventData.Shift)), out shift)
                 && reader.TryGetField<char>(naming.Get(nameof(KeyboardEventData.KeyChar)), out keyChar)
               )
            {
                result = new KeyboardEventData
                (
                    eventType,
                    alt,
                    control,
                    keyCode,
                    keyData,
                    keyValue,
                    shift,
                    keyChar,
                    deviceData.Timestamp
                );

                return true;
            }

            return false;
        }


        protected override void Write(KeyboardEventData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize<DeviceData>(writer, data);

            writer.WriteField(data.EventType);
            writer.WriteField(data.Alt);
            writer.WriteField(data.Control);
            writer.WriteField(data.Shift);
            writer.WriteField(data.KeyCode);
            writer.WriteField(data.KeyData);
            writer.WriteField(data.KeyValue);
            writer.WriteField(data.Modifiers);
            writer.WriteField(data.KeyChar);
        }
    }
}
