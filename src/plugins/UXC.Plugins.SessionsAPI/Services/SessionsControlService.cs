using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using AutoMapper;
using UXC.Plugins.SessionsAPI.Entities;
using UXC.Plugins.SessionsAPI.Recording;
using UXC.Sessions;
using UXC.Sessions.Recording.Local;
using UXC.Sessions.Timeline;
using UXI.Common.Extensions;

namespace UXC.Plugins.SessionsAPI.Services
{
    public class SessionsControlService
    {
        private readonly ExternalSessionDefinitions _definitions;
        private readonly ISessionsControl _control;
        private readonly IMapper _mapper;

        public SessionsControlService(ExternalSessionDefinitions definitions, ISessionsControl control, IMapper mapper)
        {
            _definitions = definitions;
            _control = control;
            _mapper = mapper;
        }

        public SessionRecordingInfo GetCurrentRecording()
        {
            SessionRecordingInfo recordingInfo = null;

            var recording = _control.CurrentRecording;
            if (recording != null)
            {
                recordingInfo = _mapper.Map<SessionRecordingInfo>(recording);
                recordingInfo.Id = ApiRoutes.Recording.ConvertToRouteString(recording.OpenedAt);
            }

            return recordingInfo;
        }           


        public bool Continue()
        {
            var recording = _control.CurrentRecording;
            if (recording != null)
            {
                return recording.Continue();
            }

            return false;
        }


        public bool Continue(SessionStep step)
        {
            var recording = _control.CurrentRecording;
            if (recording != null)
            {
                return recording.Continue(step);
            }

            return false;
        }


        public void Open(int definitionId)
        {
            SessionDefinition definition;
            if (_definitions.Definitions.TryGet(d => d.Id == definitionId, out definition))
            {
                var recording = _control.Record(definition);

                recording.OpenAsync(CancellationToken.None).Forget();
            }
            throw new ArgumentOutOfRangeException(nameof(definitionId));
        }


        public void Open(SessionDefinitionCreate session)
        {
            session.ThrowIfNull(nameof(session));

            var definition = _mapper.Map<SessionDefinition>(session);

            if (definition.Recorders.Any(r => r.Name.Equals("Local", StringComparison.CurrentCultureIgnoreCase)) == false)
            {
                definition.Recorders.Add(new SessionRecorderDefinition("Local"));
            }

            if (session.Save)
            {
                _definitions.Add(definition);
            }

            var recording = _control.Record(definition);

            recording.OpenAsync(CancellationToken.None).Forget();
        }


        public void Clear()
        {
            _control.Clear();
        }
    }
}
