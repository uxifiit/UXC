using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UXI.Common.Converters;

namespace UXC.Sessions.Common.Converters
{
    class EmptyStringToVisibilityConverter : BoolToValueConverter<Visibility>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isEmpty = String.IsNullOrWhiteSpace(value as string);

            return base.Convert(isEmpty, targetType, parameter, culture);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
