namespace UXC.Sessions.Timeline.Actions
{
    public class WelcomeActionSettings : QuestionaryActionSettings
    {
        public bool Ignore { get; set; } 

        public string CustomTitle { get; set; }

        public bool HideDescription { get; set; }

        public Text Description { get; set; }

        public bool HideDevices { get; set; }

        public string StartButtonLabel { get; set; }
    }

   
   
}
