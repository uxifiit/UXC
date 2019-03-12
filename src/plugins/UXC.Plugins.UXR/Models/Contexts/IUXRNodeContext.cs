/**
 * UXC.Plugins.UXR
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;

namespace UXC.Plugins.UXR.Models
{
    public interface IUXRNodeContext
    {
        bool IsConnected { get; }
        event EventHandler<bool> IsConnectedChanged;
        
        int NodeId { get; }
        event EventHandler<int> NodeIdChanged;

        string NodeName { get; }
        event EventHandler<string> NodeNameChanged;
    }
}
