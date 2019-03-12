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
    public class WrappedCommand : ICommand
    {
        private ICommand _linkedCommand;
        private object _defaultParameter;

        public event EventHandler CanExecuteChanged;


        public WrappedCommand(ICommand linkedCommand)
        {
            if (linkedCommand == null)
            {
                throw new ArgumentNullException(nameof(linkedCommand));
            }

            _linkedCommand = linkedCommand;
            _linkedCommand.CanExecuteChanged += linkedCommand_CanExecuteChanged;
        }

        public WrappedCommand(ICommand linkedCommand, object defaultParameter)
            : this(linkedCommand)
        {
            _defaultParameter = defaultParameter;
        }


        private void linkedCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }


        public bool CanExecute(object parameter)
        {
            return _linkedCommand.CanExecute(parameter ?? _defaultParameter);
        }


        public void Execute(object parameter)
        {
            _linkedCommand.Execute(parameter ?? _defaultParameter);
        }
    }
}
