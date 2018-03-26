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

        //protected override bool ConnectInternal(Plugin item)
        //{
        //    base.ConnectInternal(item);

        //    _modules.Connect(item.Module);

        //    return true;

        //    //if (module != null)
        //    //{
        //    //    var types = _service.Registrations;
        //    //    foreach (var type in types)
        //    //    {
        //    //        var matchingBindings = module.Bindings.Where(b => b.Service.Equals(type));
        //    //        if (matchingBindings.Any())
        //    //        {
        //    //            _service.NotifyRegisteredTypes(type, kernel.GetAll(type));
        //    //        }
        //    //        throw new NotImplementedException();
        //    //    }
        //    //}

        //}

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
