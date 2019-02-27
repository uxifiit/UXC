namespace UXC.Sessions.Timeline.Actions
{
    public class WriteAnswerQuestionActionSettings : QuestionActionSettings
    {
        public string ValidAnswerRegexPattern { get; set; }

        public double? Width { get; set; }

        public double? Height { get; set; }

        public bool IsMultiline { get; set; } = false;
    }

   
   
}
