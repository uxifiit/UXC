using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using UXC.Core.Devices;

namespace UXC.Common.Converters
{
    public class DeviceStateToBrushConverter : DependencyObject, IValueConverter
    {
        public Brush DisconnectedBrush
        {
            get { return (Brush)GetValue(DisconnectedBrushProperty); }
            set { SetValue(DisconnectedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisconnectedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisconnectedBrushProperty =
            DependencyProperty.Register(nameof(DisconnectedBrush), typeof(Brush), typeof(DeviceStateToBrushConverter), new PropertyMetadata(new SolidColorBrush()));



        public Brush ReadyBrush
        {
            get { return (Brush)GetValue(ReadyBrushProperty); }
            set { SetValue(ReadyBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReadyBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReadyBrushProperty =
            DependencyProperty.Register(nameof(ReadyBrush), typeof(Brush), typeof(DeviceStateToBrushConverter), new PropertyMetadata(new SolidColorBrush()));



        public Brush ConnectedBrush
        {
            get { return (Brush)GetValue(ConnectedBrushProperty); }
            set { SetValue(ConnectedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CalibrationBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectedBrushProperty =
            DependencyProperty.Register(nameof(ConnectedBrush), typeof(Brush), typeof(DeviceStateToBrushConverter), new PropertyMetadata(new SolidColorBrush()));



        public Brush RecordingBrush
        {
            get { return (Brush)GetValue(RecordingBrushProperty); }
            set { SetValue(RecordingBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RecordingBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecordingBrushProperty =
            DependencyProperty.Register(nameof(RecordingBrush), typeof(Brush), typeof(DeviceStateToBrushConverter), new PropertyMetadata(new SolidColorBrush()));



        public Brush ErrorBrush
        {
            get { return (Brush)GetValue(ErrorBrushProperty); }
            set { SetValue(ErrorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorBrushProperty =
            DependencyProperty.Register(nameof(ErrorBrush), typeof(Brush), typeof(DeviceStateToBrushConverter), new PropertyMetadata(new SolidColorBrush()));




        public Brush DefaultBrush
        {
            get { return (Brush)GetValue(DefaultBrushProperty); }
            set { SetValue(DefaultBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultBrushProperty =
            DependencyProperty.Register(nameof(DefaultBrush), typeof(Brush), typeof(DeviceStateToBrushConverter), new PropertyMetadata(new SolidColorBrush()));






        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DeviceState state = (DeviceState)value;
            switch (state)
            {
                case DeviceState.Disconnected:
                    return DisconnectedBrush;
                case DeviceState.Connected:
                    return ConnectedBrush;
                //case DeviceState.Ready:
                    //return ReadyBrush;
                case DeviceState.Recording:
                    return RecordingBrush;
                case DeviceState.Error:
                    return ErrorBrush;
            }

            return DefaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
