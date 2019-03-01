using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using UXI.Common;
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Core;

namespace UXC.Devices.Adapters
{
    internal class DeviceObservables : DisposableBase, IObservableDevice
    {
        private IDisposable _statesReplay;

        private DeviceObservables(DeviceType type, Type dataType, IObservable<DeviceStateChange> states, IObservable<DeviceData> data, IObservable<Exception> errors, IObservable<LogMessage> logs)
        {
            var replayedStates = states.Replay(1);

            DeviceType = type;
            DataType = dataType;
            States = replayedStates;
            Data = data;
            ConnectionErrors = errors;
            Logs = logs;

            _statesReplay = replayedStates.Connect();
        }

        public DeviceType DeviceType { get; }

        public Type DataType { get; }

        public IObservable<DeviceStateChange> States { get; }

        public IObservable<DeviceData> Data { get; }

        public IObservable<Exception> ConnectionErrors { get; }

        public IObservable<LogMessage> Logs { get; }


        public static IObservableDevice CreateForDevice(DeviceAdapter adapter, IDevice device)
        {
            var states = Observable.FromEventPattern<DeviceStateChangedEventHandler, DeviceStateChangedEventArgs>(
                h => adapter.StateChanged += h,
                h => adapter.StateChanged -= h)
                .Select(d => new DeviceStateChange(d.EventArgs.CurrentValue, d.EventArgs.Timestamp))
                .StartWith(new DeviceStateChange(adapter.State, DateTime.Now));
           
            var data = Observable.FromEventPattern<DeviceDataReceivedEventHandler, DeviceData>(
                h => device.Data += h,
                h => device.Data -= h)
                .Select(d => d.EventArgs);

            var errors = Observable.FromEventPattern<ErrorOccurredEventHandler, Exception>(
                h => device.ConnectionError += h,
                h => device.ConnectionError -= h)
                .Select(d => d.EventArgs);

            var logMessages = Observable.FromEventPattern<DeviceLogEventHandler, LogMessage>(
                h => device.Log += h,
                h => device.Log -= h)
                .Select(d => d.EventArgs);

            var logsWithDump = Observable.Create<LogMessage>(observer =>
            {
                var subscription = logMessages.Subscribe(observer);
                device.DumpInfo();
                return subscription;
            });

            return new DeviceObservables(adapter.Code.DeviceType, device.DataType, states, data, errors, logsWithDump); 
        }


        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    _statesReplay?.Dispose();
                    _statesReplay = null;
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
