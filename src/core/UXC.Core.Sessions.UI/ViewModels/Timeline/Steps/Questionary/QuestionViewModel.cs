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
        internal QuestionViewModel(string question, string id, IQuestionAnswerViewModel answer, bool isRequired)
        {
            Question = question;
            Id = id;
            Answer = answer;
            IsRequired = isRequired;
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
    }
}
