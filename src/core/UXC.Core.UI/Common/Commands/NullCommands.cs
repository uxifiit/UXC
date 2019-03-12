/**
 * UXC.Core.UI
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
using System.Windows.Input;

namespace UXC.Core.Common.Commands
{
    public class NullCommand : ICommand
    {
        public static readonly NullCommand Instance = new NullCommand();

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public bool CanExecute(object parameter) => false;

        public void Execute(object parameter) { }
    }
}
