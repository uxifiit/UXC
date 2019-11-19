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
    public class Point3CsvConverter : CsvConverter<Point3>
    {
        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref Point3 result)
        {
            double x;
            double y;
            double z;

            if (
                    reader.TryGetField<double>(naming.Get(nameof(Point3.X)), out x)
                 && reader.TryGetField<double>(naming.Get(nameof(Point3.Y)), out y)
                 && reader.TryGetField<double>(naming.Get(nameof(Point3.Z)), out z)
               )
            {
                result = new Point3(x, y, z);

                return true;
            }

            return false;
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(Point3.X)));
            writer.WriteField(naming.Get(nameof(Point3.Y)));
            writer.WriteField(naming.Get(nameof(Point3.Z)));
        }


        protected override void Write(Point3 data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.X);
            writer.WriteField(data.Y);
            writer.WriteField(data.Z);
        }
    }
}
