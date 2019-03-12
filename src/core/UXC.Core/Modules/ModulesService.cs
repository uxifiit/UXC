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
using Ninject;
using Ninject.Syntax;

namespace UXC.Core.Modules
{
    internal class ModulesService : IModulesService, IModulesNotifier
    {
        private readonly MultiValueDictionary<WeakReference, Action<IEnumerable<object>>> _targets = new MultiValueDictionary<WeakReference, Action<IEnumerable<object>>>();
        private readonly MultiValueDictionary<Type, Action<IEnumerable<object>>> _registrations = new MultiValueDictionary<Type, Action<IEnumerable<object>>>();

        public IEnumerable<Type> Registrations => _registrations.Keys;

        public void Register<T>(object target, Action<IEnumerable<T>> callback)
        {
            Type type = typeof(T);

            WeakReference reference = new WeakReference(target);

            // TODO convert to WeakAction to avoid memory leaks
            // WeakAction<IEnumerable<T>> weakCallback = new WeakAction<IEnumerable<T>>(callback);
            Action<IEnumerable<object>> action = new Action<IEnumerable<object>>((o) => callback.Invoke(o?.OfType<T>() ?? Enumerable.Empty<T>()));

            _registrations.Add(type, action);
            _targets.Add(reference, action);
        }

        public void NotifyRegisteredTypes(Type type, IEnumerable<object> instances)
        {
            IReadOnlyCollection<Action<IEnumerable<object>>> clients;
            if (_registrations.TryGetValue(type, out clients))
            {
                foreach (var client in clients)
                {
                    client.Invoke(instances);
                }
            }
        }
    }
}
