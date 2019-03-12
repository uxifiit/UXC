/**
 * UXC.Core.Sessions.UI
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

namespace UXC.Sessions.Views
{
    /// <summary>
    /// Interaction logic for SessionRecordingControl.xaml
    /// </summary>
    public partial class SessionRecordingControl : UserControl
    {
        public SessionRecordingControl()
        {
            InitializeComponent();
            this.Unloaded += SessionRecordingControl_Unloaded;
        }

        private void SessionRecordingControl_Unloaded(object sender, RoutedEventArgs e)
        {
            MahApps.Metro.Controls.Dialogs.DialogParticipation.SetRegister(this, null);
        }
    }
}
