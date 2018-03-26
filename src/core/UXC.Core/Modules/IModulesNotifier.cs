using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Syntax;

namespace UXC.Core.Modules
{
    public interface IModulesNotifier
    {
        IEnumerable<Type> Registrations { get; }

        void NotifyRegisteredTypes(Type type, IEnumerable<object> instances);
    }
}
