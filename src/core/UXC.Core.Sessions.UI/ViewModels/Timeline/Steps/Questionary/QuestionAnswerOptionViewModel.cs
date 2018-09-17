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
