using System;

namespace UXC.Sessions.Timeline.Actions
{
    public class CloseProgramActionSettings : ExecutedActionSettingsBase
    {
        public bool ForceClose { get; set; }

        public TimeSpan? ForceCloseTimeout { get; set; }

        public string Message { get; set; }
    }
}
