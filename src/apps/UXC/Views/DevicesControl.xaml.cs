/**
 * UXC
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
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
using UXC.Core.ViewModels.Adapters;

namespace UXC.Views
{
    /// <summary>
    /// Interaction logic for DevicesControl.xaml
    /// </summary>
    public partial class DevicesControl : UserControl
    {
        public DevicesControl()
        {
            InitializeComponent();
        }

        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button button = (Button)sender;
            AdapterViewModel avm = button.DataContext as AdapterViewModel;
            if (avm != null)
            {
                var menu = ContextMenuService.GetContextMenu(button);
                menu.DataContext = avm;
                menu.IsOpen = true;
            }
        }
    }
}
