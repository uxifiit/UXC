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
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.ViewModels.Timeline.Steps.Questionary
{
    internal class ChooseAnswerQuestionViewModel : IQuestionAnswerViewModel
    {
        private readonly ChooseAnswerQuestionActionSettings _settings;

        internal ChooseAnswerQuestionViewModel(ChooseAnswerQuestionActionSettings settings)
        {
            _settings = settings;

            Options = settings.Answers.Select(a => new QuestionAnswerOptionViewModel(a)).ToList();
        }


        public string Id => _settings.Id;


        public bool IsMultiChoice => _settings.Limit.HasValue == false || _settings.Limit.Value > 1;


        public ICollection<QuestionAnswerOptionViewModel> Options { get; }


        public bool Validate()
        {
            int count = Options.Count(a => a.IsChecked);

            return (_settings.IsRequired == false || count > 0)
                && (_settings.Limit.HasValue == false || count <= _settings.Limit)
                && (_settings.Minimum.HasValue == false || count >= _settings.Minimum.Value);
        }


        public string GetAnswer()
        {
            var indexes = Enumerable.Range(0, Options.Count);

            return Options.Zip(indexes, (a, i) => a.IsChecked ? i.ToString() : null)
                          .Where(i => i != null)
                          .DefaultIfEmpty(String.Empty)
                          .Aggregate((x, y) => x + ";" + y);
        }


        public void Reset()
        {
            Options.ForEach(a => a.IsChecked = false);
        }
    }
}
