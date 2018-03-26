using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Models.Contexts
{
    public interface IContext<TInfo>
    {
        TInfo GetInfo();
    }
}
