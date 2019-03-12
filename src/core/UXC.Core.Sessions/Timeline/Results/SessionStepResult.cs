/**
 * UXC.Core.Sessions
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;
using UXI.Common.Extensions;

namespace UXC.Sessions.Timeline.Results
{
    public enum SessionStepResultType
    {
        Successful,
        Failed,
        Skipped
    }


    public class SessionStepResult
    {
        public static readonly SessionStepResult Successful = new SessionStepResult(SessionStepResultType.Successful);
        public static readonly SessionStepResult Failed = new SessionStepResult(SessionStepResultType.Failed);
        public static readonly SessionStepResult Skipped = new SessionStepResult(SessionStepResultType.Skipped);

        protected SessionStepResult(SessionStepResultType result)
        {
            ResultType = result;
        }

        public SessionStepResultType ResultType { get; }
    }


    public class SessionStepExceptionResult : SessionStepResult
    {
        public SessionStepExceptionResult(Exception ex)
            : base(SessionStepResultType.Failed)
        {
            Exception = ex;
        }

        public Exception Exception { get; }
    }


    public class QuestionarySessionStepResult : SessionStepResult
    {
        public QuestionarySessionStepResult(IEnumerable<QuestionAnswer> answers) 
            : base(SessionStepResultType.Successful)
        {
            Answers = answers.ThrowIfNull(nameof(answers)).ToList();
        }

        public List<QuestionAnswer> Answers { get; }
    }


    public class QuestionAnswer
    {
        public QuestionAnswer(string tag, string answer)
        {
            QuestionId = tag;
            Answer = answer;
        }

        public string QuestionId { get; }
        public string Answer { get; }
    }
}
