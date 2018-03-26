using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Debug.Fixations.Observers;
using Ninject.Modules;
using UXC.Observers;

namespace Debug.Fixations
{
    public class DebugFixationsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDeviceObserver>().To<FixationsObserver>().InSingletonScope();
        }
    }
}
