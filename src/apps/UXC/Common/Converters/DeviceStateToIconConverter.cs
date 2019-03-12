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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UXC.Core.Devices;

namespace UXC.Common.Converters
{
    public class DeviceStateToIconConverter : IValueConverter
    {
        public static Icon Convert(DeviceState state)
        {
            switch (state)
            {
                case DeviceState.Disconnected:
                    return Properties.Resources.Grey;
                case DeviceState.Connected:
                    return Properties.Resources.Orange;
                //case DeviceState.Ready:
                case DeviceState.Recording:
                    return Properties.Resources.Green;
                case DeviceState.Error:
                    return Properties.Resources.Red;
              
            }
            return Properties.Resources.Grey;
        }


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DeviceState state = (DeviceState)value;
            return Convert(state);
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
