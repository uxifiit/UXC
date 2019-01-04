using MahApps.Metro;
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

namespace UXC.Views
{
    /// <summary>
    /// Interaction logic for AboutControl.xaml
    /// </summary>
    public partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            InitializeComponent();

            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(this.Resources, theme.Item2, theme.Item1);
        }


        public string VersionNumber
        {
            get { return (string)GetValue(VersionNumberProperty); }
            set { SetValue(VersionNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VersionNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VersionNumberProperty =
            DependencyProperty.Register("VersionNumber", typeof(string), typeof(AboutControl), new PropertyMetadata(null));



        public Visibility DebugLabelVisibility
        {
            get { return (Visibility)GetValue(DebugLabelVisibilityProperty); }
            set { SetValue(DebugLabelVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DebugLabelVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DebugLabelVisibilityProperty =
            DependencyProperty.Register("DebugLabelVisibility", typeof(Visibility), typeof(AboutControl), new PropertyMetadata(Visibility.Collapsed));



        public Visibility AdminLabelVisibility
        {
            get { return (Visibility)GetValue(AdminLabelVisibilityProperty); }
            set { SetValue(AdminLabelVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AdminLabelVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AdminLabelVisibilityProperty =
            DependencyProperty.Register("AdminLabelVisibility", typeof(Visibility), typeof(AboutControl), new PropertyMetadata(Visibility.Collapsed));
    }
}
