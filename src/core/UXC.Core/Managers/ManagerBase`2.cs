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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common;
using UXI.Common.Extensions;

namespace UXC.Core.Managers
{
    public abstract class ManagerBase<TItem, TCollection> : IManager<TItem>
        where TItem : class
        where TCollection : IList<TItem>, new()
    {
        protected TCollection _connections = new TCollection();

        private IEnumerable<TItem> connections = null;
        public IEnumerable<TItem> Connections
        {
            get { return connections ?? (connections = new ReadOnlyCollection<TItem>(_connections)); }
        }

        public bool Connect(TItem item)
        {
            item.ThrowIfNull(nameof(item));

            bool connected = CanConnect(item) ? ConnectInternal(item) : false;

            OnConnected(item, connected);

            return connected;
        }

        protected virtual void OnConnected(TItem item, bool success)
        {
            if (success)
            {
                ConnectionsChanged?.Invoke(this, CollectionChangedEventArgs<TItem>.CreateForAddedItem(item));
            }
        }

        public IEnumerable<TItem> Connect(params TItem[] items)
        {
            return ConnectMany(items);
        }


        protected virtual bool CanConnect(TItem item)
        {
            return _connections.Contains(item) == false;
        }


        protected virtual bool ConnectInternal(TItem item)
        {
            _connections.Add(item);
            return true;
        }
        

        public IEnumerable<TItem> ConnectMany(IEnumerable<TItem> items)
        {
            items.ThrowIfNull(nameof(items));

            var connected = items.Where(a => CanConnect(a))
                                 .Where(a => ConnectInternal(a))
                                 .ToList();

            OnConnectedMany(connected);

            return new ReadOnlyCollection<TItem>(connected);
        }


        protected virtual void OnConnectedMany(IEnumerable<TItem> connected)
        {
            if (connected.Any())
            {
                ConnectionsChanged?.Invoke(this, CollectionChangedEventArgs<TItem>.CreateForAddedCollection(connected));
            }
        }


        public bool Disconnect(TItem item)
        {
            item.ThrowIfNull(nameof(item));

            bool disconnected = DisconnectInternal(item);

            OnDisconnected(item, disconnected);

            return disconnected;
        }


        protected virtual void OnDisconnected(TItem item, bool disconnected)
        {
            if (disconnected)
            {
                ConnectionsChanged?.Invoke(this, CollectionChangedEventArgs<TItem>.CreateForRemovedItem(item));
            }
        }


        public IEnumerable<TItem> Disconnect(params TItem[] items)
        {
            return DisconnectMany(items);
        }


        protected virtual bool CanDisconnect(TItem item)
        {
            return _connections.Contains(item);
        }


        protected virtual bool DisconnectInternal(TItem item)
        {
            _connections.Remove(item);
            return true;
        }


        public IEnumerable<TItem> DisconnectMany(IEnumerable<TItem> items)
        {
            items.ThrowIfNull(nameof(items));

            var disconnected = items.Where(a => CanDisconnect(a))
                                    .Where(a => DisconnectInternal(a))
                                    .ToList();

            OnDisconnectedMany(disconnected);

            return new ReadOnlyCollection<TItem>(disconnected);
        }


        protected virtual void OnDisconnectedMany(IEnumerable<TItem> disconnected)
        {
            if (disconnected.Any())
            {
                ConnectionsChanged?.Invoke(this, CollectionChangedEventArgs<TItem>.CreateForRemovedCollection(disconnected));
            }
        }

        public IEnumerable<TItem> DisconnectAll()
        {
            return DisconnectMany(Connections.ToList());
        }


        public event EventHandler<CollectionChangedEventArgs<TItem>> ConnectionsChanged;

     

        #region IEnumerable<TItem> Members

        public IEnumerator<TItem> GetEnumerator()
        {
            return Connections.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Connections.GetEnumerator();
        }

        #endregion
    }
}
