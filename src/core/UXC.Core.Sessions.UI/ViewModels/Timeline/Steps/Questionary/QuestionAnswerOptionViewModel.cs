/**
 * UXC.Core.Sessions.UI
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

namespace UXC.Sessions.ViewModels.Timeline.Steps.Questionary
{
    internal class QuestionAnswerOptionViewModel
    {
        public QuestionAnswerOptionViewModel(string answer)
        {
            Answer = answer;
            //Tag = tag;
        }

        public bool IsChecked { get; set; } = false;

        public string Answer { get; }

        //public string Tag { get; }
    }
}
