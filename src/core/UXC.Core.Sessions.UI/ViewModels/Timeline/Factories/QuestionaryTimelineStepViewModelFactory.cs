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


    class ChooseQuestionAnswerViewModelFactory : RelayViewModelFactory<ChooseQuestionAnswerActionSettings, IQuestionAnswerViewModel>
    {
        public ChooseQuestionAnswerViewModelFactory()
            : base(settings => new ChooseQuestionAnswerViewModel(settings))
        {
        }
    }


    class WriteQuestionAnswerViewModelFactory : RelayViewModelFactory<WriteQuestionAnswerActionSettings, IQuestionAnswerViewModel>
    {
        public WriteQuestionAnswerViewModelFactory()
            : base(settings => new WriteQuestionAnswerViewModel(settings))
        {
        }
    }
}
