using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;
using UXC.Devices.ExternalEvents.Entities;
using UXI.Common;

namespace UXC.Devices.ExternalEvents
{
    public class EventsRecorder : DisposableBase, IEventsRecorder, IDisposable
    {
        private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly Subject<ExternalEventData> _recorder = new Subject<ExternalEventData>();
        private readonly BehaviorSubject<IObservable<ExternalEventData>> _observables
            = new BehaviorSubject<IObservable<ExternalEventData>>(Observable.Empty<ExternalEventData>());

        public IObservable<ExternalEventData> Events => _observables.Switch();

        public bool IsOpen { get; private set; }


        public void Open()
        {
            _observables.OnNext(_recorder);
            IsOpen = true;
        }


        public void Close()
        {
            _observables.OnNext(Observable.Empty<ExternalEventData>());
            IsOpen = false;
        }


        private static DateTime ConvertDateTime(long milliseconds)
        {
            return EPOCH.AddMilliseconds(milliseconds).ToLocalTime();
        }


        public void Record(ExternalEvent ev)
        {
            if (_recorder.IsDisposed == false)
            {
                DateTime timestamp = ev.Timestamp.HasValue ? ConvertDateTime(ev.Timestamp.Value) : DateTime.Now;
                DateTime? validTill = ev.ValidTill.HasValue ? ConvertDateTime(ev.ValidTill.Value) : new DateTime?();

                var data = new ExternalEventData(ev.System, ev.Name, (string)ev.Data.Value ?? String.Empty, timestamp, validTill);
                _recorder.OnNext(data);
            }
        }


        #region IDisposable Members

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    _observables.OnCompleted();

                    _recorder.Dispose();
                    _observables.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
