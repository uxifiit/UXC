﻿/**
 * UXC.Core.Sessions
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System.Collections.Generic;
using System.Linq;

namespace UXC.Sessions.Timeline.Actions
{
    public class ChooseAnswerQuestionActionSettings : QuestionActionSettings
    {
        public List<string> Answers { get; set; }

        public int? Limit { get; set; } = 1;

        public int? Minimum { get; set; }

        public override SessionStepActionSettings Clone()
        {
            var clone = (ChooseAnswerQuestionActionSettings)base.Clone();

            clone.Answers = Answers?.ToList();

            return clone;
        }
    }
}
