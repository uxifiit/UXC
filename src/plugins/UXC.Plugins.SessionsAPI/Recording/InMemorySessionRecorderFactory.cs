using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Managers;
using UXC.Core.Modules;
using UXC.Sessions;
using UXC.Sessions.Recording;

namespace UXC.Plugins.SessionsAPI.Recording
{
    class InMemorySessionRecorderFactory : ISessionRecorderFactory
    {
        private readonly IObserversManager _observers;
        private readonly IInstanceResolver _resolver;

        public InMemorySessionRecorderFactory(IObserversManager observers, IInstanceResolver resolver)
        {
            _observers = observers;
            _resolver = resolver;
        }


        public string Target => "Buffer";


        public ISessionRecorder Create(SessionRecording recording)
        {
            return new InMemorySessionRecorder(recording, _observers, _resolver.Get<InMemoryRecordingDataSource>());
        }
    }
}
