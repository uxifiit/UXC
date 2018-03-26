using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Plugins.UXR.Models.Uploads
{
    public class Upload
    {
        public Upload(SessionRecordingData recording)
        {
            Recording = recording.Clone();
            Status = UploadStatus.CreateScheduled();
        }

        public SessionRecordingData Recording { get; }
        public UploadStatus Status { get; private set; }

        public void UpdateStatus(UploadStatus status)
        {
            Status = status;
            StatusChanged?.Invoke(this, status);
        }

        public event EventHandler<UploadStatus> StatusChanged;
    }
}
