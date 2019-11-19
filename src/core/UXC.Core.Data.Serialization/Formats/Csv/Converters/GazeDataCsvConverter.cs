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
    public class GazeDataCsvConverter : CsvConverter<GazeData>
    {
        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref GazeData result)
        {
            DeviceData deviceData;
            long trackerTicks;
            GazeDataValidity validity;
            EyeGazeData leftEye;
            EyeGazeData rightEye;

            if (
                    TryGetMember<DeviceData>(reader, serializer, naming, out deviceData)
                 
                 && reader.TryGetField<long>(naming.Get(nameof(GazeData.TrackerTicks)), out trackerTicks)
                 && reader.TryGetField<GazeDataValidity>(naming.Get(nameof(GazeData.Validity)), out validity)
                 
                 && TryGetMember<EyeGazeData>(reader, serializer, naming, "Left", out leftEye)
                 && TryGetMember<EyeGazeData>(reader, serializer, naming, "Right", out rightEye)
               )
            {
                result = new GazeData(validity, leftEye, rightEye, trackerTicks, deviceData.Timestamp);

                return true;
            }

            return false;
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<DeviceData>(writer, naming);

            writer.WriteField(naming.Get(nameof(GazeData.TrackerTicks)));
            writer.WriteField(naming.Get(nameof(GazeData.Validity)));

            serializer.WriteHeader<EyeGazeData>(writer, naming, "Left");
            serializer.WriteHeader<EyeGazeData>(writer, naming, "Right");
        }


        protected override void Write(GazeData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize<DeviceData>(writer, data);

            writer.WriteField(data.TrackerTicks);
            writer.WriteField(data.Validity);

            serializer.Serialize<EyeGazeData>(writer, data.LeftEye);
            serializer.Serialize<EyeGazeData>(writer, data.RightEye);
        }
    }
}
