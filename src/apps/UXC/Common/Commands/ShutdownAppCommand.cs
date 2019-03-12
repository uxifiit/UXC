/**
 * UXC
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;

namespace UXC.Common.Commands
{
    public class ShutdownAppCommand : RelayCommand
    {
        public ShutdownAppCommand()
            : base(() => Application.Current.Shutdown())
        { }
    }
}
