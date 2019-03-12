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
    public interface IAppConfiguration
    {
        bool HideOnClose { get; set; }

        bool Experimental { get; set; }
    }
}
