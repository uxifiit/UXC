using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Modules
{

    /// <summary>
    /// Lets other consumers register for further type bindings in Ninject kernel. 
    /// If the type which the consumer registers for is bound later on, it is notified with all its instances. 
    /// </summary>
    public interface IModulesService
    {
        void Register<T>(object target, Action<IEnumerable<T>> callback);
        void Unregister(object target);
    }
}
