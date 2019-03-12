/**
 * UXC
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
namespace UXC.Configuration
{
    public interface IServerConfiguration
    {
        uint ServerPort { get; set; }
        bool SslEnabled { get; set; }
        bool SignalREnabled { get; set; }
        string CustomHostName { get; set; }
    }
}
