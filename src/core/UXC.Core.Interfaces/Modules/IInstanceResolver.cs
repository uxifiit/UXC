using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Modules
{
    public interface IInstanceResolver
    {
        T Get<T>();

        IEnumerable<T> GetAll<T>();
    }
}
