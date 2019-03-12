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
   
    public sealed class InstanceResolver : IInstanceResolver
    {
        private readonly IResolutionRoot _resolution;

        public InstanceResolver(IResolutionRoot resolution)
        {
            _resolution = resolution;
        }

        public T Get<T>()
        {
            return _resolution.Get<T>();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _resolution.GetAll<T>();
        }
    }
}
