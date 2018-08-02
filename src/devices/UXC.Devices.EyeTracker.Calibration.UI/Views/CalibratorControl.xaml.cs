using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UXC.Devices.EyeTracker.ViewModels;
using UXI.Common.Converters;

namespace UXC.Devices.EyeTracker.Views
{
    /// <summary>
    /// Interaction logic for CalibrationWindow.xaml
    /// </summary>
    public partial class CalibratorControl : UserControl
    {
        public CalibratorControl()
        {
            InitializeComponent();
        }


        public Brush EyesDisplayBackground
        {
            get { return (Brush)GetValue(EyesDisplayBackgroundProperty); }
            set { SetValue(EyesDisplayBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EyesDisplayBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EyesDisplayBackgroundProperty =
            DependencyProperty.Register("EyesDisplayBackground", typeof(Brush), typeof(CalibratorControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        

        private void EyeDistanceCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var converter = (RelativeToAbsolutePositionConverter)element.Resources["DistanceCaretPositionConverter"];

            converter.Maximum = e.NewSize.Height;
        }
    }
}
