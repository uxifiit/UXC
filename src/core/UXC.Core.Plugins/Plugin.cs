using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;

namespace UXC.Core.Plugins
{
    public class Plugin 
    {
        public INinjectModule Module { get; internal set; }
        public string Assembly { get; internal set; }
        public Version Version { get; internal set; }
        public DateTime CreatedAt { get; internal set; }
    }
}
