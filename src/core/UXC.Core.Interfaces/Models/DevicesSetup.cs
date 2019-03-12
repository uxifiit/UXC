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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Core.Devices;

namespace UXC.Models
{
    public class DevicesSetup : IDevicesSetup
    {
        public DevicesSetup(IEnumerable<DeviceType> devices) 
        {
        }

        public DevicesSetup(IEnumerable<DeviceType> devices, IDictionary<DeviceType, IDictionary<string, object>> configurations)
        {
            Devices = devices?.ToArray() ?? Enumerable.Empty<DeviceType>();
            Configurations = configurations;            
        }

        public DevicesSetup(IDevicesSetup setup)
            : this(setup.Devices, setup.Configurations)
        {   
            
        }

        public IEnumerable<DeviceType> Devices
        {
            get;
        }

        public IDictionary<DeviceType, IDictionary<string, object>> Configurations
        {
            get;
        }
    }
}
