using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using UXC.Core.Managers;
using UXC.Core.Plugins;

namespace UXC.Core.Plugins.Managers
{
    public interface IPluginsManager : IManager<Plugin>   // TODO replace with plugin?
    {
    }
}
