/**
 * UXC.Core.UI
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Common.Commands
{
    public class MenuItemCommand : RelayCommand
    {
        public string Label { get; private set; }

        public MenuItemCommand(string label, Action execute)
            : base(execute)
        {
            Label = label;
        }

        public MenuItemCommand(string label, Action execute, Func<bool> canExecute) 
            : base(execute, canExecute) 
        {
            Label = label;
        }
    }

    public class MenuItemCommand<T> : RelayCommand<T>
    {
        public string Label { get; private set; }

        public MenuItemCommand(string label, Action<T> execute)
            : base(execute)
        {
            Label = label;
        }

        public MenuItemCommand(string label, Action<T> execute, Func<T, bool> canExecute)
            : base(execute, canExecute)
        {
            Label = label;
        }
    }
}
