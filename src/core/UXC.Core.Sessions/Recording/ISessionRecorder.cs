using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Configuration;

namespace UXC.Sessions.Recording
{               
    public interface ISessionRecorder : IConfigurable, IDisposable
    {
        void Record();
        //void Close();
        event EventHandler<ISessionRecordingResult> Closed;
    }
}
