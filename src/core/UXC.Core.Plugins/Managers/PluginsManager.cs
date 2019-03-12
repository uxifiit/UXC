/**
 * UXC.Core.Plugins
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Extensions.ChildKernel;
using Ninject.Modules;
using UXC.Core.Managers;
using UXC.Core.Modules;

namespace UXC.Core.Plugins.Managers
{
    public class PluginsManager : ManagerBase<Plugin>, IPluginsManager
    {
        private readonly ModulesManager _modules;

        public PluginsManager(ModulesManager modules)
        {
            _modules = modules;
        }


        protected override bool CanConnect(Plugin plugin)
        {
            return base.CanConnect(plugin)
                && _modules.Any(m => m.Name == plugin.Module.Name) == false;
        }


        protected override void OnConnected(Plugin item, bool success)
        {
            if (success)
            {
                _modules.Connect(item.Module);

                base.OnConnected(item, success);
            }
        }


        protected override void OnConnectedMany(IEnumerable<Plugin> connected)
        {
            if (connected.Any())
            {
                _modules.ConnectMany(connected.Select(p => p.Module));

                base.OnConnectedMany(connected);
            }
        }

        
        protected override bool DisconnectInternal(Plugin item)
        {
            base.DisconnectInternal(item);

            _modules.Disconnect(item.Module);

            return true;
          
            //{
            //    IKernel kernel = null;
            //    if (_kernels.TryRemove(item.Name, out kernel))
            //    {
            //        // TODO kernel.Dispose?
            //        kernel.Unload(item.Name);
            //        return true;
            //    }               
            //}
            //return false;
        }
    }
}
