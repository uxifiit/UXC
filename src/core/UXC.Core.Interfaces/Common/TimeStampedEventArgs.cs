using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Common.Events
{
    public abstract class TimestampedEventArgs : EventArgs
    {
        public DateTime Timestamp { get; private set; }

        protected TimestampedEventArgs(DateTime timestamp)
        {
            Timestamp = timestamp;
        }
    }
}
