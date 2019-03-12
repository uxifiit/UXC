/**
 * UXC.Core.Sessions
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
using UXI.Common;

namespace UXC.Sessions
{
    public interface ISessionDefinitionsSource
    {
        IEnumerable<SessionDefinition> Definitions { get; }

        event EventHandler<CollectionChangedEventArgs<SessionDefinition>> DefinitionsChanged;

        Task RefreshAsync(CancellationToken cancellationToken);
    }
}
