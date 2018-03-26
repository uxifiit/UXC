using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXC.Core.Data;
using UXI.Common.Extensions;

namespace UXC.Devices.ExternalEvents
{
    public class ExternalEventsDevice : IDevice
    {
        private readonly IEventsRecorder _recorder;
        private SerialDisposable _subscription;

        public ExternalEventsDevice(IEventsRecorder recorder)
        {
            _recorder = recorder;

            Code = DeviceCode.Create(this, DeviceType.EXTERNAL_EVENTS.Code)
                             .ConnectionType(DeviceConnectionType.SystemApi)
                             .RunsOnMainThread(false)
                             .Build();
        }

        public DeviceCode Code { get; }

        public Type DataType => typeof(ExternalEventData);

        public event DeviceDataReceivedEventHandler Data;
        public event ErrorOccurredEventHandler ConnectionError { add { } remove { } }
        public event DeviceLogEventHandler Log { add { } remove { } }

        public bool ConnectToDevice()
        {
            _subscription = new SerialDisposable();

            return true;
        }

        public bool DisconnectDevice()
        {
            _recorder.Close();

            _subscription?.Dispose();

            return true;
        }

        public void DumpInfo()
        {
            
        }

        public bool StartRecording()
        {
            if (_subscription != null)
            {
                _subscription.Disposable = _recorder.Events.Subscribe(data => Data?.Invoke(this, data));

                _recorder.Open();

                return true;
            }

            return false;
        }

        public bool StopRecording()
        {
            if (_subscription != null)
            {
                _subscription.Disposable = Disposable.Empty;

                _recorder.Close();
            }

            return true;
        }
    }
}
