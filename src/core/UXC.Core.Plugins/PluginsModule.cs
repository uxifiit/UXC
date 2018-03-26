using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;
using UXC.Core.Managers;
using UXC.Core.Plugins;
using UXC.Core.Plugins.Configuration;
using UXC.Core.Plugins.Managers;
using UXC.Models.Contexts;

namespace UXC.Core.Plugins
{
    public class PluginsModule : NinjectModule
    {
        public PluginsModule() { }
        public PluginsModule(PluginsLoadingMode mode)
        {
            Mode = mode;
        }

        public PluginsLoadingMode Mode { get; } = PluginsLoadingMode.Implicit;

        public IList<INinjectModule> Plugins { get; } = new List<INinjectModule>();

        public string Directory { get; set; } = PluginsConstants.PLUGIN_DIRECTORY;

        public override void Load()
        {
            Bind<IPluginsManager>().To<PluginsManager>().InSingletonScope();
#if DEBUG
            bool design = Kernel.Get<IAppContext>().IsDesign;
            if (design == false)
            {
#endif
                if (Mode.HasFlag(PluginsLoadingMode.Implicit))
                {
                    LoadImplicitPluginsLoading();
                }
#if DEBUG
            }
#endif

            if (Mode.HasFlag(PluginsLoadingMode.Explicit))
            {
                LoadExplicitPluginsLoading();
            }
        }

        private void LoadExplicitPluginsLoading()
        {
            Bind<ILoader>().To<ExplicitPluginsLoader>().InSingletonScope().WithConstructorArgument("plugins", Plugins);
        }

        private void LoadImplicitPluginsLoading()
        {
            if (Kernel.GetBindings(typeof(IPluginsConfiguration)).Any() == false)
            {
                Bind<IPluginsConfiguration>().To<PluginsConfiguration>().InSingletonScope();
            }

            Bind<IPluginsService>().To<PluginsService>().InSingletonScope().WithConstructorArgument("directory", Directory);
            Bind<ILoader>().To<AssemblyPluginsLoader>().InSingletonScope();
        }
    }
}
