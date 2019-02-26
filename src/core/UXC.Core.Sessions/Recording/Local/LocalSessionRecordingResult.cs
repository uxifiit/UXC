using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Sessions.Recording.Local
{
    public class LocalSessionRecordingResult : ISessionRecordingResult
    {
        internal LocalSessionRecordingResult(SessionRecording recording, string rootFolder)
        {
            Recording = recording;
            RootFolder = rootFolder;
            Paths = new List<string>();
        }

        public SessionRecording Recording { get; }

        public string RootFolder { get; }

        public ICollection<string> Paths { get; }

        public bool IsClosed { get; private set; }

        internal void Close()
        {
            IsClosed = true;
        }
    }
}
