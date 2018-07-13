using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Core.Data.Serialization;
using UXC.Devices.Adapters;
using UXC.Observers;
using UXC.Sessions.Serialization;

namespace UXC.Sessions.Recording.Local
{
    internal class LocalSessionDeviceRecorder : IDeviceObserver
    {
        private const int DATA_BUFFER_SIZE = 1000;

        private readonly List<DeviceType> _devices;
        private readonly SessionRecordingPathsBuilder _paths;
        private readonly IDataSerializationFactory _writerFactory;
        private readonly LocalSessionRecordingResult _result;
        private readonly IObservable<SessionRecordingEvent> _sessionEvents;
        private readonly IObservable<SessionRecordingEvent> _sessionRecordingEvents;
        private readonly IScheduler _scheduler;

        internal LocalSessionDeviceRecorder
        (
            SessionRecording recording, 
            IDataSerializationFactory writerFactory, 
            SessionRecordingPathsBuilder paths, 
            LocalSessionRecordingResult result,
            IScheduler scheduler
        )
        {
            _sessionEvents = recording.Events;
            _devices = recording.SelectedDevices.ToList();
            _writerFactory = writerFactory;
            _paths = paths;
            _result = result;
            _scheduler = scheduler;

            _sessionRecordingEvents = _sessionEvents.FirstOrDefaultAsync(e => e.State == SessionState.Processing || e.State == SessionState.Completed);
        }

        public IDisposable Connect(IObservableDevice device)
        {
            string extension = _writerFactory.FileExtension;

            IDisposable data = Disposable.Empty;
            if (device.DataType.IsSubclassOf(typeof(DeviceData)))
            {
                string deviceDataPath = _paths.BuildDeviceFilePath(device.DeviceType, "data", extension);
                data = RecordForSessionWithWriter(device.Data, deviceDataPath);
                             //.TakeUntilOtherCompletes(_sessionRecordingEvents)
                             //.AttachWriter(deviceDataPath, _writerFactory, device.RecordingDataType, DATA_BUFFER_SIZE)
                             //.ObserveOn(scheduler: _scheduler)
                             //.Subscribe();
                _result.Paths.Add(deviceDataPath);
            }


            string deviceStatesPath = _paths.BuildDeviceFilePath(device.DeviceType, "states", extension);
            IDisposable states = RecordForSessionWithWriter(device.States, deviceStatesPath);
                                       //.TakeUntilOtherCompletes(_sessionRecordingEvents)
                                       //.AttachWriter(deviceStatesPath, _writerFactory)
                                       //.ObserveOn(scheduler: _scheduler)
                                       //.Subscribe();
            _result.Paths.Add(deviceStatesPath);


            string deviceLogsPath = _paths.BuildDeviceFilePath(device.DeviceType, "log", extension);
            IDisposable logs = RecordForSessionWithWriter(device.Logs, deviceLogsPath);
                                     //.TakeUntilOtherCompletes(_sessionRecordingEvents)
                                     //.AttachWriter(deviceLogsPath, _writerFactory)
                                     //.ObserveOn(scheduler: _scheduler)
                                     //.Subscribe();
            _result.Paths.Add(deviceLogsPath);

            return new CompositeDisposable(data, states, logs);
        }


        private IDisposable RecordForSessionWithWriter<T>(IObservable<T> observable, string path)
        {
            return observable.TakeUntilOtherCompletes(_sessionRecordingEvents)
                             .AttachWriter(path, _writerFactory)
                             .ObserveOn(scheduler: _scheduler)
                             .Subscribe();
        }


        public bool IsDeviceSupported(DeviceType type)
        {
            return _devices.Contains(type);
        }
    }
}
