namespace UXC.Sessions.Timeline.Actions
{
    public abstract class QuestionActionSettings : SessionStepActionSettings
    {
        public string Id { get; set; }

        public Text Question { get; set; }

        public bool IsRequired { get; set; } = true;

        public string HelpText { get; set; }

        public override SessionStepActionSettings Clone()
        {
            return (QuestionActionSettings)this.MemberwiseClone();
        }
    }
}
