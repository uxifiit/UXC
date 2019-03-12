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

namespace UXC.Core.Modules
{

    /// <summary>
    /// Lets other consumers register for further type bindings in Ninject kernel. 
    /// If the type which the consumer registers for is bound later on, it is notified with all its instances. 
    /// </summary>
    public interface IModulesService
    {
        void Register<T>(object target, Action<IEnumerable<T>> callback);
    }
}
