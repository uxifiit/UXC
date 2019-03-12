/**
 * GazeVisualization
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GazeVisualization.Common.Converters
{
    public class FixationFilterOptionsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string name = value?.GetType().Name ?? String.Empty;
            return name.Replace("Options", "")
                       .Aggregate
                       (
                            new StringBuilder(),
                            (builder, a) => { if (Char.IsUpper(a)) { builder.Append(" " + a); } else { builder.Append(a); } return builder; },
                            builder => builder.ToString()
                       )
                       .TrimStart();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
