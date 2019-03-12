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
using UXI.Common;

namespace UXC.Core.Managers
{
    public interface IManager<T> : IEnumerable<T>
        where T : class
    {            
        /// <summary>
        /// Gets all the connected items in the current manager.
        /// </summary>
        IEnumerable<T> Connections { get; }

        /// <summary>
        /// Attempts to connect a new item into the current manager if it is not already connected. 
        /// Passing a null values is not allowed. 
        /// </summary>
        /// <param name="item">Item to connect.</param>
        /// <returns>true if the connection was successful, otherwise false.</returns>
        bool Connect(T item);

        /// <summary>
        /// TODO documentation
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        IEnumerable<T> Connect(params T[] items);

        /// <summary>
        /// TODO documentation
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        IEnumerable<T> ConnectMany(IEnumerable<T> items);

        /// <summary>
        /// TODO documentation
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Disconnect(T item);

        /// <summary>
        /// TODO documentation
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        IEnumerable<T> Disconnect(params T[] items);

        /// <summary>
        /// TODO documentation
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        IEnumerable<T> DisconnectMany(IEnumerable<T> items);

        /// <summary>
        /// TODO documentational
        /// </summary>
        event EventHandler<CollectionChangedEventArgs<T>> ConnectionsChanged;
    }
}
