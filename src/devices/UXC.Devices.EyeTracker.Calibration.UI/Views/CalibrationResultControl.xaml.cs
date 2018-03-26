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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UXC.Core.Common.Converters;
using UXC.Core.Controls;
using UXC.Devices.EyeTracker.ViewModels;

namespace UXC.Devices.EyeTracker.Views
{
    /// <summary>
    /// Interaction logic for CalibrationResultControl.xaml
    /// </summary>
    public partial class CalibrationResultControl : UserControl
    {
        public CalibrationResultControl()
        {
            InitializeComponent();

            var refLeftConverter = (ReferenceValueConverter)Resources["LeftPositionConverterReference"];
            refLeftConverter.Source = leftEyeTruePositionsDisplay.LeftPositionConverter;

            var refTopConverter = (ReferenceValueConverter)Resources["TopPositionConverterReference"];
            refTopConverter.Source = leftEyeTruePositionsDisplay.TopPositionConverter;
        }
    }
}
