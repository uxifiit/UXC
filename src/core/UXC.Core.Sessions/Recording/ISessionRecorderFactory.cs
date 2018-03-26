using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Sessions.Recording
{
    public interface ISessionRecorderFactory
    {
        string Target { get; }

        ISessionRecorder Create(SessionRecording recording);
    }
}
