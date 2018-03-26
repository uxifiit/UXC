using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Sessions
{
    public enum SessionState
    {
        None,

        Opening,

        Preparing,

        Running,

        Processing,

        Completed,

        Cancelled
    }
}
