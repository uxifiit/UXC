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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UXC.Sessions.Timeline.Actions;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels.Timeline.Steps.Questionary
{
    internal class WriteAnswerQuestionViewModel : BindableBase, IQuestionAnswerViewModel
    {
        private readonly WriteAnswerQuestionActionSettings _settings;

        internal WriteAnswerQuestionViewModel(WriteAnswerQuestionActionSettings settings)
        {
            _settings = settings;
        }


        public string Id => _settings.Id;


        private string answer = String.Empty;
        public string Answer
        {
            get { return answer; }
            set { Set(ref answer, value); }
        }


        public double Width => _settings.Width.HasValue 
                             ? _settings.Width.Value 
                             : 300;


        public double Height => _settings.IsMultiline 
                              ? (_settings.Height ?? 120)
                              : Double.NaN;


        public bool IsMultiline => _settings.IsMultiline;


        public System.Windows.TextWrapping TextWrapping => _settings.IsMultiline
                                                         ? System.Windows.TextWrapping.Wrap 
                                                         : System.Windows.TextWrapping.NoWrap;


        public string GetAnswer() => Answer;


        public bool Validate()
        {
            return (String.IsNullOrWhiteSpace(Answer) == false)
                && (String.IsNullOrWhiteSpace(_settings.ValidAnswerRegexPattern)
                    || Regex.IsMatch(Answer, _settings.ValidAnswerRegexPattern));
        }


        public void Reset()
        {
            Answer = String.Empty;
        }
    }
}
