using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Plugins.UXR.Models.Uploads
{
    public enum UploadStep
    {
        Scheduled,
        Preparing,
        Uploading,
        Verifying,
        Completing,
        Completed
    }

    public class UploadStatus
    {
        private UploadStatus(UploadStep step, bool isIndeterminate)
        {
            Step = step;
            IsIndeterminate = isIndeterminate;
        }

        public UploadStep Step { get; private set; } = UploadStep.Scheduled;

        public bool IsIndeterminate { get; } = true;

        public int Progress { get; private set; } = 0;

        public void Update(int i)
        {
            if (IsIndeterminate == false)
            {
                Progress = i;
            }
        }

        public static UploadStatus CreateScheduled() => new UploadStatus(UploadStep.Scheduled, false);
        public static UploadStatus CreatePreparing() => new UploadStatus(UploadStep.Preparing, false);
        public static UploadStatus CreateUploading() => new UploadStatus(UploadStep.Uploading, false);
        public static UploadStatus CreateVerifying() => new UploadStatus(UploadStep.Verifying, true);
        public static UploadStatus CreateCompleting() => new UploadStatus(UploadStep.Completing, true);
        public static UploadStatus CreateCompleted() => new UploadStatus(UploadStep.Completed, false) { Progress = 100 };
    }
}
