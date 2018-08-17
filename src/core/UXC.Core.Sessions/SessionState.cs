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


    public static class SessionStateEx
    {
        public static bool IsRunningState(this SessionState state)
        {
            return state == SessionState.Preparing
                || state == SessionState.Running
                || state == SessionState.Processing;
        }
    }
}
