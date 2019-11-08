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
    public class Point2CsvConverter : CsvConverter<Point2>
    {
        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref Point2 result)
        {
            double x;
            double y;

            if (
                    reader.TryGetField<double>(naming.Get(nameof(Point2.X)), out x)
                 && reader.TryGetField<double>(naming.Get(nameof(Point2.Y)), out y)
               )
            {
                result = new Point2(x, y);

                return true;
            }

            return false;
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(Point2.X)));
            writer.WriteField(naming.Get(nameof(Point2.Y)));
        }


        protected override void Write(Point2 data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.X);
            writer.WriteField(data.Y);
        }
    }
}
