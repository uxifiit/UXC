using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Plugins.UXR.Models.Uploads
{
    public interface IUploader
    {
        Upload CurrentUpload { get; }
        bool IsEnabled { get; }
        bool IsWorking { get; }

        event EventHandler<Upload> CurrentUploadChanged;
        event EventHandler<bool> IsEnabledChanged;
        event EventHandler<bool> IsWorkingChanged;

        void Start();
        void Stop();
    }
}
