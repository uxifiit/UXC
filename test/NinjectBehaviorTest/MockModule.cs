using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Ninject.Syntax;

namespace NinjectBehaviorTest
{
    class MockModule : NinjectModule
    {
        private readonly Action<IBindingRoot> _load;

        public MockModule(Action<IBindingRoot> loadAction)
        {
            _load = loadAction;
        }

        public override void Load()
        {
            _load.Invoke(this);
        }
    }
}
