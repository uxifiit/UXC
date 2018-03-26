using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Devices.Streamers.Configuration
{
    internal static class ConfigurationSections
    {
        public const string SECTION_DEFAULT = "Devices.Streamers";
        public const string SECTION_SCREENCAST = SECTION_DEFAULT + ".ScreenCast";
        public const string SECTION_AUDIO = SECTION_DEFAULT + ".Audio";
        public const string SECTION_VIDEO = SECTION_DEFAULT + ".Video";
        public const string SECTION_FFMPEG = SECTION_DEFAULT + ".FFmpeg";
    }
}
