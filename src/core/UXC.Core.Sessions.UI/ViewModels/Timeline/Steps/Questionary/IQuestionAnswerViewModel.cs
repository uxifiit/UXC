using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Sessions.ViewModels.Timeline.Steps.Questionary
{
    public interface IQuestionAnswerViewModel
    {
        bool Validate();

        string GetAnswer();

        void Reset();
    }
}
