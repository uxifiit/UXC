using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Devices.Streamers
{
    public class FFmpegDevice
    {
        public FFmpegDevice(FFmpegStreamType type, string name)
        {
            Type = type;
            Name = name;
        }

        public string Name { get; }

        public FFmpegStreamType Type { get; }
        //public string AlternativeName { get; }
    }
}
