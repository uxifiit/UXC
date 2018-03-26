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
