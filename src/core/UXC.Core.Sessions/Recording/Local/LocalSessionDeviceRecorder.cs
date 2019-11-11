/**
 * UXC.Core.Sessions
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
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
using UXI.Serialization;
using UXI.Serialization.Reactive;

namespace UXC.Sessions.Recording.Local
{
    internal class LocalSessionDeviceRecorder : IDeviceObserver
    {
        private const int DATA_BUFFER_SIZE = 1000;

        private readonly List<DeviceType> _devices;
        private readonly SessionRecordingPathsBuilder _paths;
        private readonly DataIO _io;
        private readonly LocalSessionRecordingResult _result;
        private readonly IObservable<SessionRecordingEvent> _sessionEvents;
        private readonly IObservable<SessionRecordingEvent> _sessionRecordingEvents;
        private readonly IScheduler _scheduler;

        internal LocalSessionDeviceRecorder
        (
            SessionRecording recording, 
            DataIO io, 
            SessionRecordingPathsBuilder paths, 
            LocalSessionRecordingResult result,
            IScheduler scheduler
        )
        {
            // recording.Definition.SerializationFormat
            //FileFormat format = _io.Formats.Keys.Where(f => f.ToString().Equals())
            _sessionEvents = recording.Events;
            _devices = recording.SelectedDevices.ToList();
            _io = io;
            _paths = paths;
            _result = result;
            _scheduler = scheduler;

            _sessionRecordingEvents = _sessionEvents.FirstOrDefaultAsync(e => e.State == SessionState.Processing || e.State == SessionState.Completed);
        }

        public IDisposable Connect(IObservableDevice device)
        {
            //string extension = _writerFactory.FileExtension;
            FileFormat targetFormat = FileFormat.CSV;
            string extension = targetFormat.ToString().ToLower();

            IDisposable data = Disposable.Empty;
            if (device.DataType.IsSubclassOf(typeof(DeviceData)))
            {
                string deviceDataPath = _paths.BuildDeviceFilePath(device.DeviceType, "data", extension);
                data = RecordForSessionWithWriter(device.Data, deviceDataPath, device.DataType, targetFormat);
                             
                _result.Paths.Add(deviceDataPath);
            }


            string deviceStatesPath = _paths.BuildDeviceFilePath(device.DeviceType, "states", "json");  // TODO Serialization hard-coded
            IDisposable states = RecordForSessionWithWriter(device.States, deviceStatesPath, typeof(DeviceStateChange), FileFormat.JSON);
                                       
            _result.Paths.Add(deviceStatesPath);


            string deviceLogsPath = _paths.BuildDeviceFilePath(device.DeviceType, "log", "json");
            IDisposable logs = RecordForSessionWithWriter(device.Logs, deviceLogsPath, typeof(Core.LogMessage), FileFormat.JSON);
                                    
            _result.Paths.Add(deviceLogsPath);

            return new CompositeDisposable(data, states, logs);
        }


        private IDisposable RecordForSessionWithWriter(IObservable<object> observable, string path, Type dataType, FileFormat format)
        {
            return observable.TakeUntilOtherCompletes(_sessionRecordingEvents)
                             .AttachWriter(path, dataType, _io, format)
                             .ObserveOn(scheduler: _scheduler)
                             .Subscribe();
        }


        public bool IsDeviceSupported(DeviceType type)
        {
            return _devices.Contains(type);
        }
    }
}
