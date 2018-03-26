using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UXC.Core.Devices;

namespace UXC.Common.Converters
{
    public class DeviceTypeToSymbolConverter : IValueConverter
    {

        private static readonly Dictionary<DeviceType, string> map = new Dictionary<DeviceType, string>()
        {
            { DeviceType.Physiological.EYETRACKER, "\uE052" },
            { DeviceType.Input.MOUSE, "\uE1E3" },
            { DeviceType.Input.KEYBOARD, "\uE144" },
            { DeviceType.Streaming.SCREENCAST, "\uE147" },
            { DeviceType.Streaming.WEBCAM_AUDIO, "\uE1D6" },
            { DeviceType.Streaming.WEBCAM_VIDEO, "\uE116" },
            //{ DeviceType.Physiological.EMOTION, "\uE170" },
            //{ DeviceType.Physiological.EPOC, "\U0001F4A1" },
            { DeviceType.EXTERNAL_EVENTS, "\u270E" }
        };

        #region IValueConverter Members

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

        #endregion
    }
}
