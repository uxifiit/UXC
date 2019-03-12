/**
 * UXC.Core
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;

namespace UXC.Core.Managers
{
    public class AdaptersManager : ManagerBase<IDeviceAdapter>, IAdaptersManager
    {
        protected override bool CanConnect(IDeviceAdapter adapter)
        {
            return base.CanConnect(adapter) && _connections.Any(c => c.Code.DeviceType.Equals(adapter.Code.DeviceType)) == false;
        }

        #region IDisposable Members

        public void Dispose()   // TODO Dispose adapters: This was just a quick fix, dispose should not be here probably, but in the COnnector.
        {
            var conns = _connections.ToList();
            _connections.Clear();
            if (conns.Any())
            {
                foreach (var c in conns)
                {
                    c.Dispose();
                }
            }
        }

        #endregion
    }
}
