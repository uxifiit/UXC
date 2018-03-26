using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Sessions.Timeline.Results
{
    class FixationFilterResult : SessionStepResult
    {
        public FixationFilterResult(string filename)
            : base(SessionStepResultType.Successful)
        {
            Filename = filename;
        }

        public string Filename { get; }
    }
}
