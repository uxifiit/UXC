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
using System.Windows.Data;
using UXC.Core.Devices;

namespace UXC.Common.Converters
{
    public class DeviceTypeToNameConverter : IValueConverter
    {

        private static readonly Dictionary<DeviceType, string> map = new Dictionary<DeviceType, string>()
        {
            { DeviceType.Physiological.EYETRACKER, "Eye Tracker" },
            { DeviceType.Input.MOUSE, "Mouse" },
            { DeviceType.Input.KEYBOARD, "Keyboard" },
            { DeviceType.Streaming.SCREENCAST, "Screencast" },
            { DeviceType.Streaming.WEBCAM_AUDIO, "Webcam Audio" },
            { DeviceType.Streaming.WEBCAM_VIDEO, "Webcam Video" },
            //{ DeviceType.Physiological.EMOTION, "Emotions" },
            //{ DeviceType.Physiological.EPOC, "EPOC" },
            { DeviceType.EXTERNAL_EVENTS, "External Events" }
        };


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DeviceType type = (DeviceType)value;
            if (map.ContainsKey(type))
            {
                return map[type];
            }
            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
