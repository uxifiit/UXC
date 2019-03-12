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
using System.Windows.Interactivity;
using MahApps.Metro.Controls;

namespace UXC.Common.Actions
{
    public class HideWindowAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            try
            {
                AssociatedObject?.Invoke(() => Window.GetWindow(AssociatedObject)?.Hide());
            }
            catch { }
        }
    }
}
