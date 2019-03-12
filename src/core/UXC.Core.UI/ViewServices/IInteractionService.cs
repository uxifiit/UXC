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

namespace UXC.Core.ViewServices
{
    public interface IInteractionService
    {
        event EventHandler<InteractionRequestEventArgs> InteractionRequested;

        bool RequestInteraction(object source, InteractionRequest request);
    }
}
