using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Logging
{
    public class NullLogger : ILogger
    {
        private static NullLogger instance = null;
        public static NullLogger Instance => instance ?? (instance = new NullLogger());

        public bool IsDebugEnabled => false;

        public bool IsErrorEnabled => false;

        public bool IsFatalEnabled => false;

        public bool IsInfoEnabled => false;

        public bool IsWarnEnabled => false;

        public void Debug(object message, string caller = null)
        {
        }

        public void Debug(object message, Exception exception, string caller = null)
        {
        }

        public void Error(object message, string caller = null)
        {
        }

        public void Error(object message, Exception exception, string caller = null)
        {
        }

        public void Fatal(object message, string caller = null)
        {
        }

        public void Fatal(object message, Exception exception, string caller = null)
        {
        }

        public void Info(object message, string caller = null)
        {
        }

        public void Info(object message, Exception exception, string caller = null)
        {
        }

        public void Warn(object message, string caller = null)
        {
        }

        public void Warn(object message, Exception exception, string caller = null)
        {
        }
    }
}
