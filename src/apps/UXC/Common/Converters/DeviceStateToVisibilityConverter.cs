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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXI.Common.Converters;

namespace UXC.Common.Converters
{
    public class DeviceStateToVisibilityConverter : BoolToVisibilityConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DeviceState type = (DeviceState)value;
            return base.Convert(type.ToString().Equals(parameter as string, StringComparison.InvariantCultureIgnoreCase), targetType, null, culture);
        }
    }
}
