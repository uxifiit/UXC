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
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels.Timeline.Steps.Questionary
{
    public class QuestionViewModel : BindableBase
    {
        internal QuestionViewModel(string question, string id, IQuestionAnswerViewModel answer, bool isRequired, string helpText = null)
        {
            Question = question;
            Id = id;
            Answer = answer;
            IsRequired = isRequired;
            HelpText = helpText;
        }


        public string Question { get; }


        public string Id { get; }


        public bool IsRequired { get; }


        public IQuestionAnswerViewModel Answer { get; }


        public void ResetValidity() => IsInvalid = false;


        public bool Validate()
        {
            bool isValid = IsRequired == false || Answer.Validate();
            IsInvalid = isValid == false;
            return isValid;
        }


        private bool isInvalid = false;
        public bool IsInvalid
        {
            get { return isInvalid; }
            private set { Set(ref isInvalid, value); }
        }


        public string HelpText { get; }
    }
}
