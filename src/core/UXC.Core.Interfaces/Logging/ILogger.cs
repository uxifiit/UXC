/**
 * UXC.Core.Interfaces
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Checks if this logger is enabled for the Debug level.
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the Error level.
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the Fatal level.
        /// </summary>
        bool IsFatalEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the Info level.
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the Warn level.
        /// </summary>
        bool IsWarnEnabled { get; }

        void Debug(object message, string caller = null);
        void Debug(object message, Exception exception, string caller = null);

        void Error(object message, string caller = null);
        void Error(object message, Exception exception, string caller = null);

        void Fatal(object message, string caller = null);
        void Fatal(object message, Exception exception, string caller = null);

        void Info(object message, string caller = null);
        void Info(object message, Exception exception, string caller = null);

        void Warn(object message, string caller = null);
        void Warn(object message, Exception exception, string caller = null);
    }
}
