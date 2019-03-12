/**
 * UXC.Plugins.SessionsAPI
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
using System.Threading;
using System.Threading.Tasks;
using UXC.Plugins.SessionsAPI.Entities;
using UXC.Sessions;
using UXI.Common;
using UXI.Common.Extensions;

namespace UXC.Plugins.SessionsAPI
{
    public class ExternalSessionDefinitions : ISessionDefinitionsSource
    {
        private readonly List<SessionDefinition> _definitions = new List<SessionDefinition>();

        public IEnumerable<SessionDefinition> Definitions => _definitions;


        public void Add(SessionDefinition definition)
        {
            definition.ThrowIfNull(nameof(definition));

            _definitions.Add(definition);

            DefinitionsChanged?.Invoke(this, CollectionChangedEventArgs<SessionDefinition>.CreateForAddedItem(definition));
        }


        public Task RefreshAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }


        public event EventHandler<CollectionChangedEventArgs<SessionDefinition>> DefinitionsChanged;
    }
}
