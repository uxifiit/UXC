using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Devices.Adapters.Commands
{
    public enum CommandResult
    {
        Success,
        Cancelled,
        Failed,
        UserRequired,
        NotApplied
    }
}
