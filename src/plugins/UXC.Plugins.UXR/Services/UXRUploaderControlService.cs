using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core;
using UXC.Core.Common.Events;
using UXC.Plugins.UXR.Models;
using UXC.Plugins.UXR.Models.Uploads;
using UXC.Sessions;
using UXC.Sessions.Recording.Local;
using UXI.Common.Extensions;
using UXI.Common.Helpers;

namespace UXC.Plugins.UXR.Services
{
    internal class UXRUploaderControlService : NotifyStateChangedBase<ControlServiceState>, IControlService
    {
        private readonly ISessionsControl _sessions;
        private readonly IUploader _uploader;
        private readonly UploadsQueue _uploads;
        private readonly IUXRNodeContext _uxrNode;
        private readonly UXRSessionDefinitionsSource _uxrSessions;

        private IDisposable _subscription;

        public UXRUploaderControlService(ISessionsControl sessions, IUploader uploader, UploadsQueue uploads, IUXRNodeContext uxrNode, UXRSessionDefinitionsSource uxrSessions)
        {
            _sessions = sessions;
            _uploader = uploader;
            _uploads = uploads;
            _uxrNode = uxrNode;
            _uxrSessions = uxrSessions;
        }

        public bool AutoStart => true;


        public void Start()
        {
            _subscription = _sessions.CompletedRecordings
                                     .OfType<LocalSessionRecordingResult>()
#if DEBUG
                                     .Do(r => DumpSessionInfo(r))
#endif
                                     .Where(r => r.Recording.State == SessionState.Completed)
                                     .Where(r => r.Recording.RecorderConfigurations.ContainsKey("UXR"))
                                     .Subscribe(r => UploadSessionData(r.Recording, r.RootFolder));

            _uploader.Start();
            State = ControlServiceState.Running;
        }


#if DEBUG
        private void DumpSessionInfo(LocalSessionRecordingResult result)
        {
            Debug.WriteLine($"Session '{result.Recording.Definition.Name}' [{result.Recording.State}]:");

            using (StringWriter sw = new StringWriter())
            {
                ObjectDumper.Write(result.Paths, 1, sw);
                Debug.WriteLine(sw.ToString());
            }
        }
#endif

        private void UploadSessionData(SessionRecording recording, string path)
        {
            int sessionId;

            var data = new SessionRecordingData();
            data.SessionId = _uxrSessions.TryGetUXRSessionId(recording.Definition, out sessionId) ? sessionId : new int?();
            data.Project = recording.Definition.Project;
            data.SessionName = recording.Definition.Name;
            data.RecordingId = recording.Id;
            data.StartTime = recording.StartedAt ?? new DateTime();
            
            data.Path = path;

            _uploads.TryEnqueue(data);
        }


        public void Stop()
        {
            var subscription = ObjectEx.GetAndReplace(ref _subscription, null);
            subscription?.Dispose();

            _uploader.Stop();
            State = ControlServiceState.Stopped;
        }


        public bool IsWorking() => _uploader.IsWorking || (_uploader.IsEnabled && _uploads.Uploads.Any());
    }
}
