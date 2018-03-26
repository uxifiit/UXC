using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.ViewServices
{
    [Flags]
    public enum AppState
    {
        None = 0,
        Working = 1,

        Loading = None | Working,
        Loaded = 2,

        Starting = Loaded | Working,
        Started = Loaded | 4,

        Error = 16
    }
}
