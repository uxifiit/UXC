using System;
using System.Collections.Generic;

namespace UXC.Sessions.Timeline.Actions
{
    public class LaunchProgramActionSettings : ExecutedActionSettingsBase
    {
        public string Path { get; set; }

        public string WorkingDirectoryPath { get; set; }

        public string Arguments { get; set; }

        public List<string> ArgumentsParameters { get; set; } = null;

        public bool RunInBackground { get; set; } = false;

        public bool KeepRunning { get; set; }

        public bool ForceClose { get; set; }

        public bool WaitForStart { get; set; }

        public TimeSpan? WaitTimeout { get; set; }
    }
}
