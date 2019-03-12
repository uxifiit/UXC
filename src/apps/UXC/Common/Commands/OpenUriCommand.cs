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
using GalaSoft.MvvmLight.CommandWpf;
using UXI.SystemApi;

namespace UXC.Common.Commands
{
    public class OpenUriCommand : RelayCommand<Uri>
    {
        public OpenUriCommand() 
            : base(path => ProcessInterop.CreateProcess(null, $"cmd.exe /C start {path.AbsoluteUri}", true, true))
        {
        }
    }
}
