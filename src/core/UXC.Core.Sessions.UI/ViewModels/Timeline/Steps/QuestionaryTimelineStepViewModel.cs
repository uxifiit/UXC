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
                                    .Select(q => new QuestionViewModel(q.Question, q.Id, (IQuestionAnswerViewModel)resolver.Create(q), q.IsRequired));

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

                return IsSubmitted = true;
            }

            answers = new List<QuestionAnswer>();

            return IsSubmitted = false;
        }


        private bool ValidateQuestions()
        {
            return _questions.TrueForAll(q => q.Validate());
        }
    }


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


    public interface IQuestionAnswerViewModel
    {
        bool Validate();

        string GetAnswer();

        void Reset();
    }


    internal class ChooseQuestionAnswerViewModel : IQuestionAnswerViewModel
    {
        private readonly ChooseQuestionAnswerActionSettings _settings;

        internal ChooseQuestionAnswerViewModel(ChooseQuestionAnswerActionSettings settings)
        {
            _settings = settings;

            Answers = settings.Answers.Select(a => new QuestionAnswerChoiceViewModel(a)).ToList();
        }

        public bool IsMultiChoice => _settings.Limit > 1;

        public ICollection<QuestionAnswerChoiceViewModel> Answers { get; }

        public bool Validate()
        {
            int count = Answers.Count(a => a.IsChecked);
            return (_settings.IsRequired == false || count > 0)
                && count <= _settings.Limit;
        }

        public string GetAnswer()
        {
            var indexes = Enumerable.Range(0, Answers.Count);

            return Answers.Zip(indexes, (a, i) => a.IsChecked ? i.ToString() : null)
                          .Where(i => i != null)
                          .Aggregate((x, y) => x + "," + y);
        }

        public void Reset()
        {
            Answers.ForEach(a => a.IsChecked = false);
        }
    }


    internal class QuestionAnswerChoiceViewModel
    {
        public QuestionAnswerChoiceViewModel(string answer)
        {
            Answer = answer;
            //Tag = tag;
        }

        public bool IsChecked { get; set; } = false;

        public string Answer { get; }

        //public string Tag { get; }
    }



    internal class WriteQuestionAnswerViewModel : BindableBase, IQuestionAnswerViewModel
    {
        private readonly WriteQuestionAnswerActionSettings _settings;

        internal WriteQuestionAnswerViewModel(WriteQuestionAnswerActionSettings settings)
        {
            _settings = settings;
        }

        private string answer = String.Empty;
        public string Answer
        {
            get { return answer; }
            set { Set(ref answer, value); }
        }

        public string GetAnswer() => Answer;

        public bool Validate()
        {
            return String.IsNullOrWhiteSpace(Answer) == false
                && (String.IsNullOrWhiteSpace(_settings.ValidAnswerRegexPattern)
                    || Regex.IsMatch(Answer, _settings.ValidAnswerRegexPattern));
        }

        public void Reset()
        {
            Answer = String.Empty;
        }
    }
}
