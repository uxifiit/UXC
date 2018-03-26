using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Plugins.UXR.Models.Uploads.Design
{
    public class DesignUploadsSource : IUploadsSource
    {
        private static readonly List<SessionRecordingData> Recordings = new List<SessionRecordingData>()
        {
            new SessionRecordingData() {
                Project = "New infrastructure UX case study",
                SessionName = "Pilot session",
                SessionId = 1,
                Path = @"C:\Users\ux\AppData\Local\UXC\Pilot infrastructure",
                StartTime = new DateTime(2017, 4, 20, 14, 03, 00),
            },
            new SessionRecordingData()
            {
                Project = "Pilot study",
                SessionName = "Session 1",
                SessionId = 2,
                Path = @"C:\Users\ux\AppData\Local\UXC\Session 2",
                StartTime = new DateTime(2017, 3, 31, 16, 03, 00),
            },
            new SessionRecordingData()
            {
                Project = "Testing experiment",
                SessionName = "Testing session",
                SessionId = null,
                Path = @"C:\Users\ux\AppData\Local\UXC\Session 1",
                StartTime = new DateTime(2017, 3, 20, 14, 03, 00),
            },
        };

        public IEnumerable<SessionRecordingData> Load()
        {
            return Recordings;
        }

        public bool Save(IEnumerable<SessionRecordingData> recordings)
        {
            return true;
        }
    }
}
