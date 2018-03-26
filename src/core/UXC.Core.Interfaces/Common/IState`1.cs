using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Common
{
    public interface IState<TState>
        where TState : IComparable
    {
        TState State { get; }
    }
}
