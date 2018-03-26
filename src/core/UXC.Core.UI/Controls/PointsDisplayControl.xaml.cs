using System;
using System.Collections;
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
using UXI.Common.Converters;

namespace UXC.Core.Controls
{
    /// <summary>
    /// Interaction logic for GazePage.xaml
    /// </summary>
    public partial class PointsDisplayControl : UserControl
    {
        private readonly RelativeToAbsolutePositionConverter _leftConverter;
        private readonly RelativeToAbsolutePositionConverter _topConverter;
        public PointsDisplayControl()
        {
            InitializeComponent();

            _leftConverter = (RelativeToAbsolutePositionConverter)Resources["LeftPositionConverter"];
            _topConverter = (RelativeToAbsolutePositionConverter)Resources["TopPositionConverter"];

            UpdateConverter(display.ActualWidth, display.ActualHeight);
        }


        private void display_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateConverter(e.NewSize.Width, e.NewSize.Height);
        }
        private void UpdateConverter(double width, double height)
        {
            _leftConverter.Maximum = width;
            _topConverter.Maximum = height;
        }


        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(PointsDisplayControl), new PropertyMetadata(null));



        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(PointsDisplayControl), new PropertyMetadata(null));



        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateSelectorProperty =
            DependencyProperty.Register(nameof(ItemTemplateSelector), typeof(DataTemplateSelector), typeof(PointsDisplayControl), new PropertyMetadata(null));


        public IValueConverter LeftPositionConverter => _leftConverter;
        public IValueConverter TopPositionConverter => _topConverter;
    }
}
