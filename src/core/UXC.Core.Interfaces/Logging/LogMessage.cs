using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core
{
    public class LogMessage
    {
        
        public LogMessage(LogLevel level, string tag, string message)
            :   this(level, tag, message, null)
        {   
        }

        public LogMessage(LogLevel level, string tag, string message, object content)
            : this(level, tag, message, content, DateTime.Now)
        {   
        }

        public LogMessage(LogLevel level, string tag, string message, object content, DateTime timestamp)
        {
            Tag = tag;
            Message = message;
            Content = content;
            Timestamp = timestamp;
        }

        public LogLevel Level { get; protected set; }

        public string Tag { get; protected set; }

        public string Message { get; protected set; }

        public object Content { get; protected set; }

        public DateTime Timestamp { get; protected set; }
    }
}
