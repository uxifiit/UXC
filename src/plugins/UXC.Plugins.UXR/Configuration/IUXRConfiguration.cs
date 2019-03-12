/**
 * UXC.Plugins.UXR
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
namespace UXC.Plugins.UXR.Configuration
{
    public interface IUXRConfiguration
    {
        string NodeName { get; set; }

        string EndpointAddress { get; set; }

        int StatusUpdateIntervalSeconds { get; set; }

        int UploadTimeoutSeconds { get; set; }
    }
}
