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
    public class EyeGazeDataCsvConverter : CsvConverter<EyeGazeData>
    {
        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(EyeGazeData.Validity)));

            serializer.WriteHeader<Point2>(writer, naming, nameof(EyeGazeData.GazePoint2D));
            serializer.WriteHeader<Point3>(writer, naming, nameof(EyeGazeData.GazePoint3D));
            serializer.WriteHeader<Point3>(writer, naming, nameof(EyeGazeData.EyePosition3D));
            serializer.WriteHeader<Point3>(writer, naming, nameof(EyeGazeData.EyePosition3DRelative));

            writer.WriteField(naming.Get(nameof(EyeGazeData.PupilDiameter)));
        }


        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref EyeGazeData result)
        {
            EyeGazeDataValidity validity;

            Point2 gazePoint2D;
            Point3 gazePoint3D;
            Point3 eyePosition3D;
            Point3 eyePosition3DRelative;

            double pupilDiameter;

            if (
                    reader.TryGetField<EyeGazeDataValidity>(naming.Get(nameof(EyeGazeData.Validity)), out validity)

                 && TryGetMember<Point2>(reader, serializer, naming, nameof(EyeGazeData.GazePoint2D), out gazePoint2D)
                 && TryGetMember<Point3>(reader, serializer, naming, nameof(EyeGazeData.GazePoint3D), out gazePoint3D)
                 && TryGetMember<Point3>(reader, serializer, naming, nameof(EyeGazeData.EyePosition3D), out eyePosition3D)
                 && TryGetMember<Point3>(reader, serializer, naming, nameof(EyeGazeData.EyePosition3DRelative), out eyePosition3DRelative)

                 && reader.TryGetField<double>(naming.Get(nameof(EyeGazeData.PupilDiameter)), out pupilDiameter)
               )
            {
                result = new EyeGazeData
                (
                    validity,
                    gazePoint2D,
                    gazePoint3D,
                    eyePosition3D,
                    eyePosition3DRelative,
                    pupilDiameter
                );

                return true;
            }

            return false;
        }


        protected override void Write(EyeGazeData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Validity);

            serializer.Serialize(writer, data.GazePoint2D);
            serializer.Serialize(writer, data.GazePoint3D);
            serializer.Serialize(writer, data.EyePosition3D);
            serializer.Serialize(writer, data.EyePosition3DRelative);

            writer.WriteField(data.PupilDiameter);
        }
    }
}
