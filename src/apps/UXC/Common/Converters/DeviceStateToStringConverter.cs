using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UXC.Core.Devices;

namespace UXC.Common.Converters
{
    public class DeviceStateToStringConverter : IValueConverter
    {
        private static readonly Dictionary<DeviceState, string> map = new Dictionary<DeviceState, string>()
        {
            { DeviceState.Connected, "Connected" },
            { DeviceState.Disconnected, "Disconnected" },
            { DeviceState.Error, "Error" },
            //{ DeviceState.Ready, "Ready" },
            { DeviceState.Recording, "Recording" },
        };

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DeviceState state = (DeviceState)value;
            if (map.ContainsKey(state))
            {
                return map[state];
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
