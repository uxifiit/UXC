/**
 * UXC.Core.Sessions
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UXC.Sessions
{
    public interface IProcessService
    {
        void Add(int processId, string tag);
        bool Close(string tag, bool killIfNotClosed, TimeSpan? killTimeout);
        bool CloseAll(bool killIfNotClosed, TimeSpan? killTimeout);
        void CloseProcess(Process process, bool killIfNotClosed, TimeSpan? killTimeout = null);
        bool CloseProcesses(IEnumerable<int> processIds, bool killIfNotClosed, TimeSpan? killTimeout);
        bool CloseProcessGracefully(Process process);
        void KillProcess(Process process);
        void Reset();
    }
}