using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using UXC.Plugins.SessionsAPI.Entities;
using UXC.Plugins.SessionsAPI.Services;
using UXC.Sessions.Timeline;

namespace UXC.Plugins.SessionsAPI.Hubs
{
    public class SessionRecordingHub : Hub
    {
        private readonly SessionsControlService _service;

        public SessionRecordingHub(SessionsControlService service)
        {
            _service = service;
        }


        public SessionRecordingInfo GetCurrentRecording()
        {
            return _service.GetCurrentRecording();
        }


        //public bool Start()
        //{
        //    return _service.Start();
        //}


        //public bool Stop()
        //{
        //    return _service.Stop();
        //}

        public bool Continue()
        {
            return _service.Continue();
        }

        public bool ContinueStep(SessionStep step)
        {
            return _service.Continue();
        }


        public bool OpenExistingSession(int definitionId)
        {
            try
            {
                _service.Open(definitionId);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }


        public bool OpenNewSession(SessionDefinitionCreate session)
        {
            try
            {
                _service.Open(session);
                return true;
            }
            catch (ArgumentException)
            {
                return false; // TODO ambiguous
            }
        }


        public void Clear()
        {
            _service.Clear();
        }
    }
}
