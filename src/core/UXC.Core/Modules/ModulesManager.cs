using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Extensions.ChildKernel;
using Ninject.Modules;
using Ninject.Syntax;
using UXI.Common.Extensions;
using UXC.Core.Managers;
using UXC.Managers;

namespace UXC.Core.Modules
{
    public class ModulesManager : ManagerBase<INinjectModule>
    {
        private IModulesNotifier _service;

        public ModulesManager(IKernel kernel, IModulesNotifier service)
        {
            _kernel = kernel;
            _service = service;
        }

        private IKernel _kernel;
        public IResolutionRoot Root => _kernel;

        protected override bool CanConnect(INinjectModule module)
        {
            return base.CanConnect(module) 
                && _connections.Any(m => m.Name == module.Name) == false 
                && _kernel.HasModule(module.Name) == false;
        }

        protected override bool ConnectInternal(INinjectModule module)
        {
            base.ConnectInternal(module);

            _kernel.Load(module);

            return true;
        }

        protected override void OnConnected(INinjectModule item, bool success)
        {
            base.OnConnected(item, success);

            if (success)
            {
                NotifyRegisteredTypes();
            }
        }

        protected override void OnConnectedMany(IEnumerable<INinjectModule> connected)
        {
            base.OnConnectedMany(connected);

            if (connected.Any())
            {
                NotifyRegisteredTypes();
            }
        }

        private void NotifyRegisteredTypes()
        {
            var types = _service.Registrations.ToList();
            foreach (var type in types)
            {
                _service.NotifyRegisteredTypes(type, _kernel.GetAll(type));
            }
        }

        protected override bool DisconnectInternal(INinjectModule module)
        {
            base.DisconnectInternal(module);

            string name = module.Name;
            if (_kernel.HasModule(module.Name))
            {
                _kernel.Unload(module.Name);
                return true;
            }

            return false;
        }
    }
}
