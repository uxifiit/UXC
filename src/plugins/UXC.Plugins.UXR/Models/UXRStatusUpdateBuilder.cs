using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXC.Models.Contexts;
using UXC.Sessions;
using UXR.Studies.Api.Entities.Nodes;
using UXR.Studies.Api.Entities.Sessions;
using UXR.Studies.Client;

namespace UXC.Plugins.UXR.Models
{
    public class UXRStatusUpdateBuilder
    {
        private readonly IDevicesContext _devices;
        private readonly IUXRNodeContext _node;
        private readonly ISessionsControl _sessions;
        private readonly UXRSessionDefinitionsSource _uxrSessions;


        public UXRStatusUpdateBuilder(ISessionsControl sessions, UXRSessionDefinitionsSource uxrSessions, IDevicesContext devices, IUXRNodeContext node)
        {
            _sessions = sessions;
            _uxrSessions = uxrSessions;
            _devices = devices;
            _node = node;
        }


        public NodeStatusUpdate BuildStatusUpdate()
        {
            bool isRecording = _sessions.CurrentRecording != null && _sessions.CurrentRecording.IsFinished;

            SessionRecordingUpdate recording = null;
            if (_sessions.CurrentRecording != null && _sessions.CurrentRecording.IsFinished == false)
            {
                var currentRecording = _sessions.CurrentRecording;

                int sessionId;

                recording = new SessionRecordingUpdate()
                {
                    SessionId = _uxrSessions.TryGetUXRSessionId(currentRecording.Definition, out sessionId) ? sessionId : new int?(),
                    SessionName = currentRecording.Definition.Name,
                    StartedAt = currentRecording.StartedAt,
                    Streams = currentRecording.Definition
                                              .Devices
                                              .Select(d => d.Device.Code) // TODO IsConnected, IsRecording
                                              .ToList()
                };
            }

            NodeStatusUpdate update = new NodeStatusUpdate()
            {
                Name = _node.NodeName,
                Recording = recording,
            };

            return update;
        }
    }
}
