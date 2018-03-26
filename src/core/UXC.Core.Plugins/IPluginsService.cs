using System.Collections.Generic;
using Ninject.Modules;

namespace UXC.Core.Plugins
{
    public interface IPluginsService
    {
        IEnumerable<string> GetPluginFiles();
    }
}
