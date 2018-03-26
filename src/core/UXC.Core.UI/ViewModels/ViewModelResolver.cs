using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;
using UXC.Core.Modules;

namespace UXC.Core.ViewModels
{
    public class ViewModelResolver
    {
        //private readonly IModulesService _modules; // if needed to unregister
        // TODO add multi value dict for multiple target types!!!!!!
        private readonly Dictionary<Type, IViewModelFactory> _factories = new Dictionary<Type, IViewModelFactory>();
        public ViewModelResolver(IEnumerable<IViewModelFactory> factories, IModulesService modules)
        {
            AddFactories(factories);
            modules.Register<IViewModelFactory>(this, AddFactories);
        }

        private void AddFactories(IEnumerable<IViewModelFactory> factories)
        {
            foreach (var factory in factories)
            {
                _factories.AddOrUpdate(factory.SourceType, factory, (_, __) => factory);
            }
        }

        public object Create(object source)
        {
            source.ThrowIfNull(nameof(source));

            var type = source.GetType();

            KeyValuePair<Type, IViewModelFactory> factoryBinding;
            if (_factories.TryGet(f => f.Key == type, out factoryBinding)
                || _factories.TryGet(f => type.IsSubclassOf(f.Key)
                        || (f.Key.IsInterface && f.Key.IsInstanceOfType(source))
                        || (f.Key.IsGenericTypeDefinition && type.IsGenericType && type.GetGenericTypeDefinition() == f.Key), out factoryBinding))
            {
                return factoryBinding.Value.Create(source);
            }

            throw new ArgumentOutOfRangeException(nameof(source), $"No factory found for the requested type {type.FullName}.");
        }

        public bool CanCreate(object source)
        {
            source.ThrowIfNull(nameof(source));

            var type = source.GetType();
            return _factories.ContainsKey(type);
        }
    }
}
