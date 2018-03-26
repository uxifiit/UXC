using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using UXC.Core.Plugins.Managers;

namespace UXC.Core.Plugins
{
    class AssemblyPluginsLoader : ILoader
    {
        private readonly PluginsManager _manager;
        private readonly IPluginsService _service;

        internal AssemblyPluginsLoader(PluginsManager manager, IPluginsService service)
        {
            _manager = manager;
            _service = service;
        }

        public void Load()
        {
            var files = _service.GetPluginFiles();
            if (files.Any())
            {
                foreach (var file in files.Where(f => File.Exists(f)))
                {
                    var assembly = Assembly.LoadFile(file);
                    var modules = assembly.GetExportedTypes().Where(t => t.IsAbstract == false && t.GetInterfaces().Any(i => i.Equals(typeof(INinjectModule)))).ToArray();

                    if (modules.Any())
                    {
                        Plugin plugin = new Plugin();
                        plugin.Assembly = assembly.GetName().FullName;
                        plugin.Module = (INinjectModule)Activator.CreateInstance(modules.First());
                        plugin.Version = assembly.GetName().Version;

                        _manager.Connect(plugin);
                    }
                }
            }
        }
    }

    class ExplicitPluginsLoader : ILoader
    {
        private readonly PluginsManager _manager;
        private readonly IEnumerable<INinjectModule> _modules;

        internal ExplicitPluginsLoader(PluginsManager manager, IEnumerable<INinjectModule> plugins)
        {
            _manager = manager;
            _modules = plugins?.ToList() ?? Enumerable.Empty<INinjectModule>();
        }

        public void Load()
        {
            foreach (var module in _modules)
            {
                var assembly = module.GetType().Assembly;

                Plugin plugin = new Plugin();
                plugin.Assembly = assembly.GetName().FullName;
                plugin.Module = module;
                plugin.Version = assembly.GetName().Version;

                _manager.Connect(plugin);
            }
        }
    }
}
