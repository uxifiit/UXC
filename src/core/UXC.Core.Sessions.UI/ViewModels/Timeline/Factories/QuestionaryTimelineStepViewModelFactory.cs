using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Modules;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.ViewModels.Timeline.Factories
{
    class QuestionaryTimelineStepViewModelFactory : RelayViewModelFactory<QuestionaryActionSettings, ITimelineStepViewModel>
    {
        public QuestionaryTimelineStepViewModelFactory(IInstanceResolver resolver)
            : base(settings => new QuestionaryTimelineStepViewModel(settings, resolver.Get<ViewModelResolver>()))
        {
        }
    }


    class ChooseAnswerQuestionViewModelFactory : RelayViewModelFactory<ChooseAnswerQuestionActionSettings, IQuestionViewModel>
    {
        public ChooseAnswerQuestionViewModelFactory()
            : base(settings => new ChooseAnswerQuestionViewModel(settings))
        {
        }
    }


    class WriteAnswerQuestionViewModelFactory : RelayViewModelFactory<WriteAnswerQuestionActionSettings, IQuestionViewModel>
    {
        public WriteAnswerQuestionViewModelFactory()
            : base(settings => new WriteAnswerQuestionViewModel(settings))
        {
        }
    }
}
