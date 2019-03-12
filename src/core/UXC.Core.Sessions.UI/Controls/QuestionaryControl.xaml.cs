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

namespace UXC.Sessions.Controls
{
    /// <summary>
    /// Interaction logic for QuestionaryControl.xaml
    /// </summary>
    public partial class QuestionaryControl : UserControl
    {
        public QuestionaryControl()
        {
            InitializeComponent();
        }


        public IEnumerable QuestionsSource
        {
            get { return (IEnumerable)GetValue(QuestionsProperty); }
            set { SetValue(QuestionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Questions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QuestionsProperty =
            DependencyProperty.Register("QuestionsSource", typeof(IEnumerable), typeof(QuestionaryControl), new PropertyMetadata(null));
    }
}
