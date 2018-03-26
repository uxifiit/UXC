//using System;
//using System.Linq;
//using System.Reactive.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UXI.Common;
//using UXC.Core;
//using UXC.Sessions;
//using UXC.Core.Common.Events;
//using UXI.Common.Extensions;

//namespace UXC.Services
//{
//    public class SessionsControlService : NotifyStateChangedBase<ControlServiceState>, IControlService
//    {
//        private readonly SessionDefinitionsSource _definitions;
//        private readonly ISessionsControl _sessions;

//        private IDisposable _definitionAwaiter;

//        public SessionsControlService(ISessionsControl sessions, SessionDefinitionsSource definitions)
//        {
//            _sessions = sessions;
//            _definitions = definitions;
//        }


//        public bool AutoStart => false;


//        public void Start()
//        {
//            ////auto open session on startup or wait for definition if none is registered
//            if (TryOpenSession(_definitions.Definitions.FirstOrDefault()) == false)
//            {
//                _definitionAwaiter = Observable.FromEventPattern<CollectionChangedEventArgs<SessionDefinition>>
//                    (
//                        h => _definitions.DefinitionsChanged += h,
//                        h => _definitions.DefinitionsChanged -= h
//                    )
//                    .Select(e => e.EventArgs.AddedItems)
//                    .FirstAsync(d => d.Any())
//                    .Select(d => d.First())
//                    .Subscribe(definition => TryOpenSession(definition));
//            }

//            State = ControlServiceState.Running;
//        }


//        public void Stop()
//        {
//            _definitionAwaiter?.Dispose();
//            _definitionAwaiter = null;

//            State = ControlServiceState.Stopped;
//        }


//        private bool TryOpenSession(SessionDefinition definition)
//        {
//            if (_sessions.CurrentRecording == null
//                || _sessions.CurrentRecording.IsFinished == true)
//            {
//                if (definition != null)
//                {
//                    _sessions.OpenAsync(definition).Forget();
//                    return true;
//                }
//            }
//            return false;
//        }


//        public bool IsWorking() => false;
//    }
//}
