using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using UXC.Plugins.UXR.Models.Uploads;
using UXI.Common.UI;

namespace UXC.Plugins.UXR.ViewModels.Uploads
{
    public class UploadViewModel : BindableBase
    {
        private readonly Upload _upload;
        private readonly Dispatcher _dispatcher;

        public UploadViewModel(Upload upload, Dispatcher dispatcher)
        {
            _upload = upload;
            _dispatcher = dispatcher;

            _upload.StatusChanged += (_, __) =>
            {
                _dispatcher.Invoke(() =>
                {
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(IsWorking));
                    OnPropertyChanged(nameof(IsCompleted));
                });
            };
        }

        internal Upload Upload => _upload;


        public UploadStatus Status => _upload.Status;

        public bool IsWorking => Status.Step != UploadStep.Scheduled && Status.Step != UploadStep.Completed;


        public bool IsCompleted => Status.Step == UploadStep.Completed;

        public string SessionName => _upload.Recording.SessionName;

        public string Project => _upload.Recording.Project;


        public DateTime StartTime => _upload.Recording.StartTime;


        public string Path => _upload.Recording.Path;
    }
}
