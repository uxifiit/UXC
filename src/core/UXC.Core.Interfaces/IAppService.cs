/**
 * UXC.Core.Interfaces
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using UXC.Core.Common.Events;

namespace UXC.Core
{
    public interface IAppService : INotifyStateChanged<AppState>
    {
        event EventHandler<Exception> Error;

        bool Load();
        bool Start();
        bool Stop();

        bool CheckIfStopCancelsWorkInProgress();
    }
}
