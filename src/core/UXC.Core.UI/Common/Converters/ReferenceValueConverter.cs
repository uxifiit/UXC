using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace UXC.Core.Common.Converters
{
    public class ReferenceValueConverter : DependencyObject, IValueConverter
    {
        public IValueConverter Source
        {
            get { return (IValueConverter)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(IValueConverter), typeof(ReferenceValueConverter), new PropertyMetadata(null, (d, e) => { }));


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Source?.Convert(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Source?.ConvertBack(value, targetType, parameter, culture);
        }
    }
}
