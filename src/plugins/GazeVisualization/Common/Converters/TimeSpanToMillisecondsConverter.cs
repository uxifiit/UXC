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

namespace GazeVisualization.Converters
{
    public class TimeSpanToMillisecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan? timeSpan = value as TimeSpan?;
            return timeSpan?.TotalMilliseconds ?? 0d;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? milliseconds = value as double?;
            return TimeSpan.FromMilliseconds(milliseconds ?? 0d);
        }
    }
}
