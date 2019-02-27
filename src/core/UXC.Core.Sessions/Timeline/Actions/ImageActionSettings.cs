using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Sessions.Timeline.Actions
{
    public class ImageActionSettings : ContentActionSettingsBase
    {
        public string Path { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public bool Stretch { get; set; }

        public bool ShowContinue { get; set; }

        public string ContinueButtonLabel { get; set; }
    }
}
