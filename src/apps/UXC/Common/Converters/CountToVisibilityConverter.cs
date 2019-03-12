/**
 * UXC
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
using UXI.Common.Converters;
using UXI.Common.Extensions;

namespace UXC.Common.Converters
{
    public class CountToVisibiliyConverter : BoolToVisibilityConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasAny = false;

            try
            {
                int count = System.Convert.ToInt32(value);
                hasAny = count > 0;
            }
            catch { }

            return base.Convert(hasAny, targetType, parameter, culture);
        }
    }
}
