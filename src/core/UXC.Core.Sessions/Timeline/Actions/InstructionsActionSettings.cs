using System.Collections.Generic;

namespace UXC.Sessions.Timeline.Actions
{
    public class InstructionsActionSettings : ContentActionSettingsBase
    {
        public Text Instructions { get; set; }

        public List<string> Parameters { get; set; } = null;

        public string Title { get; set; }

        public bool ShowContinue { get; set; }

        public string ContinueButtonLabel { get; set; }
    }
}
