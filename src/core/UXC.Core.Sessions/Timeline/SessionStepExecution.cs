using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Sessions.Timeline.Results;

namespace UXC.Sessions.Timeline
{
    public class SessionStepExecution
    {
        internal SessionStepExecution(SessionStep step, DateTime startedAt)
        {
            //Index = index;
            Step = step;
            StartedAt = startedAt;
        }

        //public int Index { get; }

        public SessionStep Step { get; }

        public DateTime StartedAt { get; }

        public SessionStepResult Result { get; set; }
    }
}
