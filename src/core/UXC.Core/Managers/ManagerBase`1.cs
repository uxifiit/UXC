using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Managers
{
    public abstract class ManagerBase<TItem> : ManagerBase<TItem, List<TItem>>
        where TItem : class
    {
    }
}
