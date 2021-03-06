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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UXC.Core.Common.Commands
{
    public class CancelCommand : ICommand
    {
        private readonly CancellationTokenSource _token;

        public CancelCommand(CancellationTokenSource token)
        {
            _token = token;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _token.IsCancellationRequested == false;
        }

        public void Execute(object parameter)
        {
            _token.Cancel();
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
