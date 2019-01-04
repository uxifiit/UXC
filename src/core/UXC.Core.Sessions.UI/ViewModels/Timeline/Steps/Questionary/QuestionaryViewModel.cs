using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Sessions.Timeline.Results;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels.Timeline.Steps.Questionary
{
    public class QuestionaryViewModel : BindableBase
    {
        private readonly List<QuestionViewModel> _questions;

        public QuestionaryViewModel(IEnumerable<QuestionViewModel> questions)
        {
            _questions = questions?.ToList() ?? new List<QuestionViewModel>();
        }

        public ICollection<QuestionViewModel> Questions => _questions;

        public void Reset()
        {
            IsSubmitted = false;

            foreach (var question in Questions)
            {
                question.ResetValidity();
                question.Answer.Reset();
            }
        }


        private bool isSubmitted = false;
        public bool IsSubmitted
        {
            get { return isSubmitted; }
            private set { Set(ref isSubmitted, value); }
        }


        public bool TrySubmit(out ICollection<QuestionAnswer> answers)
        {
            bool areAnswersValid = ValidateQuestions();
            if (areAnswersValid)
            {
                answers = Questions.Select(q => new QuestionAnswer(q.Id, q.Answer.GetAnswer())).ToList();
            }
            else
            {
                answers = new List<QuestionAnswer>();
            }

            IsSubmitted = areAnswersValid;

            return areAnswersValid;
        }


        private bool ValidateQuestions()
        {
            return _questions.TrueForAll(q => q.Validate());
        }
    }
}
