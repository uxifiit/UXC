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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UXI.Common;
using UXI.Common.Helpers;
using UXI.Common.Extensions;
using UXC.Devices.Adapters;
using UXC.Managers.Observers;
using UXC.Observers;
using UXC.Core.Logging;

namespace UXC.Core.Managers
{
    public class ObserversManager : ManagerBase<IDeviceObserver>, IObserversManager
    {
        private readonly ILogger _logger;
        private readonly IAdaptersManager _adapters;

        private MultiValueDictionary<IDeviceObserver, DeviceObserverConnection> _deviceObservers = new MultiValueDictionary<IDeviceObserver, DeviceObserverConnection>();

        public ObserversManager(IAdaptersManager adapters, ILogger logger)
        {
            _logger = logger;

            _adapters = adapters.ThrowIfNull(nameof(adapters));

            _adapters.ConnectionsChanged += adapters_ConnectionsChanged;
        }


        void adapters_ConnectionsChanged(object sender, CollectionChangedEventArgs<IDeviceAdapter> e)
        {
            var observers = Connections;

            if (observers.Any())
            {
                if (e.AddedItems.Any())
                {
                    foreach (var adapter in e.AddedItems)
                    {
                        foreach (var client in observers.Where(o => o.IsDeviceSupported(adapter.Code.DeviceType)))
                        {
                            ConnectObserverToDevice(client, adapter);
                        }
                    }
                }

                if (e.RemovedItems.Any())
                {
                    foreach (var adapter in e.RemovedItems)
                    {
                        foreach (var client in observers.Where(o => o.IsDeviceSupported(adapter.Code.DeviceType)))
                        {
                            DisconnectObserverFromAdapter(client, adapter);
                        }
                    }
                }
            }
        }

        private void ConnectObserverToDevice(IDeviceObserver client, IDeviceAdapter adapter)
        {
            _logger.Info(LogHelper.Prepare("Connecting " + client.ToString() + " to " + adapter.Code.DeviceType));

            IDisposable connection = client.Connect(adapter.Observables);
            _deviceObservers.Add(client, new DeviceObserverConnection(adapter, connection));
        }

        private void DisconnectObserverFromAdapter(IDeviceObserver client, IDeviceAdapter adapter)
        {
            _logger.Info(LogHelper.Prepare("Disconnecting " + client.ToString() + " from " + adapter.Code.DeviceType));

            IReadOnlyCollection<DeviceObserverConnection> connections;

            if (_deviceObservers.TryGetValue(client, out connections))
            {
                var connectionsToDelete = connections.Where(c => c.Adapter == adapter).ToList();
                foreach (var connection in connectionsToDelete)   // TODO refactor - why this loop if a single adapter?
                {
                    _deviceObservers.Remove(client, connection);
                    connection.Disconnect();
                }
            }
        }


        protected override bool ConnectInternal(IDeviceObserver client)
        {
            if (base.ConnectInternal(client))
            {
                IEnumerable<IDeviceAdapter> adapters = _adapters.Connections.Where(a => client.IsDeviceSupported(a.Code.DeviceType))
                                                                            .ToList();

                foreach (var adapter in adapters)
                {
                    ConnectObserverToDevice(client, adapter);
                }

                return true;
            }
            return false;
        }


        protected override bool DisconnectInternal(IDeviceObserver client)
        {
            if (base.DisconnectInternal(client))
            {
                IEnumerable<IDeviceAdapter> adapters = _adapters.Connections.Where(a => client.IsDeviceSupported(a.Code.DeviceType))
                                                                            .ToList();

                foreach (var adapter in adapters)
                {
                    DisconnectObserverFromAdapter(client, adapter);
                }
                return true;
            }
            return false;
        }

        #region IDisposable Members

        public void Dispose()
        {
            var conns = _deviceObservers.Values.SelectMany(c => c).ToList();
            _deviceObservers.Clear();
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
