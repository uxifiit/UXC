using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Converters;

namespace UXC.Common.Converters
{
    public class NullToVisibilityConverter : BoolToVisibilityConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return base.Convert(value != null, targetType, parameter, culture);
        }
    }
}
