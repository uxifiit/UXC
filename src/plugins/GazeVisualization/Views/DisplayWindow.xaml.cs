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
using UXI.Common.Converters;

namespace GazeVisualization.Views
{
    /// <summary>
    /// Interaction logic for DisplayWindow.xaml
    /// </summary>
    public partial class DisplayWindow : Window
    {

        private const string POSITION_CONVERTER_LEFT = "LeftPositionConverter";
        private const string POSITION_CONVERTER_TOP = "TopPositionConverter";


        public DisplayWindow()
        {
            InitializeComponent();
        }

        private RelativeToAbsolutePositionConverter LeftPositionConverter => (RelativeToAbsolutePositionConverter)Resources[POSITION_CONVERTER_LEFT];
        private RelativeToAbsolutePositionConverter TopPositionConverter => (RelativeToAbsolutePositionConverter)Resources[POSITION_CONVERTER_TOP];


        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePositionConverter(e.NewSize.Width, e.NewSize.Height);
        }

        private void UpdatePositionConverter(double width, double height)
        {
            System.Diagnostics.Debug.WriteLine($"Width {width} Height {height}");
            LeftPositionConverter.Maximum = width;
            TopPositionConverter.Maximum = height;
        }

        private void CloseSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var groups = VisualStateManager.GetVisualStateGroups(MainGrid);
            var group = groups.OfType<VisualStateGroup>().FirstOrDefault(g => g.Name == nameof(SettingsStates));
            if (group.CurrentState != null && group.CurrentState.Name == nameof(SettingsVisible))
            {
                VisualStateManager.GoToElementState(MainGrid, nameof(SettingsHidden), false);
            }
            else
            {
                VisualStateManager.GoToElementState(MainGrid, nameof(SettingsVisible), false);
            }
        }
    }
}
