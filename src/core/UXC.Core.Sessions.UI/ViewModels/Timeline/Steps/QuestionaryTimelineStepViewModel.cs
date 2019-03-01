using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;
using UXC.Sessions.ViewModels.Timeline.Steps.Questionary;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels.Timeline
{
    public class QuestionaryTimelineStepViewModel : ContentTimelineStepViewModelBase
    {
        private readonly string _id;

        private SessionRecording _recording;

        public QuestionaryTimelineStepViewModel(QuestionaryActionSettings settings, ViewModelResolver resolver)
            : base(settings)
        {
            _id = settings.Id;

            var questions = settings.Questions?
                                    .Where(q => resolver.CanCreate(q))
                                    .Select(q =>
                                    {
                                        string question = q.Question.Lines != null && q.Question.Lines.Any()
                                                        ? String.Join(Environment.NewLine, q.Question.Lines)
                                                        : String.Empty;

                                        return new QuestionViewModel(question, q.Id, (IQuestionAnswerViewModel)resolver.Create(q), q.IsRequired, q.HelpText);
                                    });

            Questionary = new QuestionaryViewModel(questions);

            HasQuestions = Questionary.Questions.Any();
        }


        public bool HasQuestions { get; }


        public QuestionaryViewModel Questionary { get; }


        private QuestionarySessionStepResult CreateResult(IEnumerable<QuestionAnswer> answers)
        {
            return new QuestionarySessionStepResult(answers);
        }

        private void TrySaveAnswersToRecordingSettings(IEnumerable<QuestionAnswer> answers)
        {
            if (String.IsNullOrWhiteSpace(_id) == false)
            {
                foreach (var qa in answers.Where(a => String.IsNullOrWhiteSpace(a.QuestionId) == false))
                {
                    _recording.Settings.SetCustomSetting(_id, qa.QuestionId, qa.Answer);
                }
            }
        }


        private SessionStepResult CompleteWithAnswers(ICollection<QuestionAnswer> answers)
        {
            var result = CreateResult(answers);

            TrySaveAnswersToRecordingSettings(answers);

            OnCompleted(result);

            return result;
        }


        public override SessionStepResult Complete()
        {
            ICollection<QuestionAnswer> answers;

            if (Questionary.TrySubmit(out answers))
            {
                return CompleteWithAnswers(answers);
            }

            return SessionStepResult.Failed;
        }


        public override void Execute(SessionRecordingViewModel recording)
        {
            _recording = recording.Recording;
        }


        private RelayCommand tryCompleteCommand;
        public RelayCommand TryCompleteCommand => tryCompleteCommand
            ?? (tryCompleteCommand = new RelayCommand(() => Complete()));
    }
}
