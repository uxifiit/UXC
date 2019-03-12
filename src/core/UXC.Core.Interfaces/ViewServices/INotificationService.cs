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
using System.Threading;
using System.Threading.Tasks;

namespace UXC.Core.ViewServices
{
    public interface INotificationService
    {
        void ShowErrorMessage(string title, string message);
        void ShowInfoMessage(string title, string message);
        Task<bool> ShowRequestMessageAsync(string title, string message, CancellationToken cancellationToken);
    }
}
