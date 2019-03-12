/**
 * UXC.Core.Sessions
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using UXC.Core.Configuration;

namespace UXC.Sessions
{
    public interface ISessionsConfiguration : IConfiguration
    {
        string TargetPath { get; }
    }
}
