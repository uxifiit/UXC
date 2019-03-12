/**
 * UXC
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using MahApps.Metro;
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
using UXC.Core.ViewModels;
using UXC.ViewModels;

namespace UXC.Views
{
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();

            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var appTheme = ThemeManager.GetAppTheme("BaseDark");
            ThemeManager.ChangeAppStyle(this.Resources, theme.Item2, appTheme);
        }



        public IEnumerable Sections
        {
            get { return (IEnumerable)GetValue(SectionsProperty); }
            set { SetValue(SectionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Sections.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SectionsProperty =
            DependencyProperty.Register("Sections", typeof(IEnumerable), typeof(SettingsControl), new PropertyMetadata(null));



        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SaveCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(SettingsControl), new PropertyMetadata(null));



        public ICommand ReloadCommand
        {
            get { return (ICommand)GetValue(ReloadCommandProperty); }
            set { SetValue(ReloadCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReloadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReloadCommandProperty =
            DependencyProperty.Register("ReloadCommand", typeof(ICommand), typeof(SettingsControl), new PropertyMetadata(null));
    }
}
