using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXC.Core.Managers;
using UXC.Core.Managers.Adapters;
using UXC.Devices.Adapters;
using UXC.Observers;
using UXC.Sessions.Serialization;
using UXC.Sessions.Recording;
using UXI.Common;
using UXI.Common.Extensions;
using UXC.Sessions.Extensions;
using System.Threading;

namespace UXC.Sessions
{
    public class SessionsControl : DisposableBase, IDisposable, ISessionsControl
    {
        private readonly IAdaptersControl _adapters;
        private readonly IDisposable _recordersSubscription;
        private readonly SessionRecorderFactoryLocator _recorders;

        private readonly ReplaySubject<SessionRecording> _recordings = new ReplaySubject<SessionRecording>();


        public SessionsControl(IAdaptersControl adapters, SessionRecorderFactoryLocator recorders)
        {
            _adapters = adapters;
            _recorders = recorders;


            CompletedRecordings = _recordings.SelectMany(r => recorders.CreateForRecording(r))
                                             .Do(r => r.Record())
                                             .SelectMany(r => Observable.FromEventPattern<ISessionRecordingResult>(h => r.Closed += h, h => r.Closed -= h)
                                                                        .FirstOrDefaultAsync()
                                                                        .Where(e => e != null)
                                                                        .Select(e => e.EventArgs))
                                             .Publish()
                                             .RefCount();

            _recordersSubscription = CompletedRecordings.Subscribe();
        }


        public SessionRecording Record(SessionDefinition definition)
        {
            // create recording from definition
            var recording = new SessionRecording(definition, _adapters);

            // close previous recording
            Close();

            // set new recording
            CurrentRecording = recording;

            return recording;
        }


        public void Close()
        {
            currentRecording?.TryCancel();
        }


        public void Clear()
        {
            Close();
            CurrentRecording = null;
        }


        private SessionRecording currentRecording = null;
        public SessionRecording CurrentRecording
        {
            get { return currentRecording; }
            private set
            {
                var old = ObjectEx.GetAndReplace(ref currentRecording, value);

                if (old != value)
                {
                    RecordingChanged?.Invoke(this, value);
                    if (value != null)
                    {
                        _recordings.OnNext(value);
                    }
                }
            }
        }

        public event EventHandler<SessionRecording> RecordingChanged;


        public IObservable<SessionRecording> Recordings => _recordings.Where(r => r.IsFinished == false);


        public IObservable<ISessionRecordingResult> CompletedRecordings { get; }


        #region IDisposable Members

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    Clear();
                    _recordersSubscription.Dispose();
                    _recordings.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
