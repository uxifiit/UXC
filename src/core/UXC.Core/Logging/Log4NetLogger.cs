using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using log4net;
using UXC.Core.Logging;

namespace UXC.Logging
{
    class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger(ILog log)
        { 
           _log = log;
        }


        public bool IsDebugEnabled => _log.IsDebugEnabled;


        public bool IsErrorEnabled => _log.IsErrorEnabled;


        public bool IsFatalEnabled => _log.IsFatalEnabled;


        public bool IsInfoEnabled => _log.IsInfoEnabled;


        public bool IsWarnEnabled => _log.IsWarnEnabled;


        public void Debug(object message, [CallerMemberName] string caller = null) 
            => _log.Debug(Prepare(message, caller));

        public void Debug(object message, Exception exception, [CallerMemberName] string caller = null) 
            => _log.Debug(Prepare(message, caller), exception);


        public void Error(object message, [CallerMemberName] string caller = null)
            => _log.Error(Prepare(message, caller));

        public void Error(object message, Exception exception, [CallerMemberName] string caller = null)
            => _log.Error(Prepare(message, caller), exception);


        public void Fatal(object message, [CallerMemberName] string caller = null)
            => _log.Fatal(Prepare(message, caller));

        public void Fatal(object message, Exception exception, [CallerMemberName] string caller = null)
            => _log.Fatal(Prepare(message, caller), exception);


        public void Info(object message, [CallerMemberName] string caller = null) 
            => _log.Info(Prepare(message, caller));

        public void Info(object message, Exception exception, [CallerMemberName] string caller = null)
            => _log.Info(Prepare(message, caller), exception);


        public void Warn(object message, [CallerMemberName] string caller = null) 
            => _log.Warn(Prepare(message, caller));

        public void Warn(object message, Exception exception, [CallerMemberName] string caller = null)
            => _log.Warn(Prepare(message, caller), exception);


        /// <summary>
        /// Prepares log message, omit caller parameter when calling to add it automatically.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="caller">Name of the caller of this method. Omit it and it will be added automatically.</param>
        /// <returns>Log message.</returns>
        private static string Prepare(object message, string caller = null)
        {
            return (caller ?? "") + "(): " + message?.ToString() ?? "[null]";
        }
    }
}
