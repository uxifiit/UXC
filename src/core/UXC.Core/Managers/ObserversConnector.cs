/**
 * UXC.Core
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Observers;
using UXC.Core.Managers;
using UXC.Core.Modules;
using UXC.Managers;

namespace UXC
{
    public class ObserversConnector : Connector<IDeviceObserver>
    {
        public ObserversConnector(IObserversManager manager, IEnumerable<IDeviceObserver> clients, IModulesService modules)
            : base(manager, clients, modules)
        {
        }
    }
}
