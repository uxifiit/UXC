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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Configuration;
using UXC.Core.Devices;
using UXC.Core.Data.Serialization;
using UXC.Core.Managers;
using UXC.Core.Modules;
using UXC.Devices.Adapters;
using UXC.Observers;
using UXC.Sessions.Serialization;
using UXI.Common;
using UXI.Common.Extensions;

namespace UXC.Sessions.Recording.Local
{
    public class LocalSessionRecorder : DisposableBase, ISessionRecorder, IDisposable
    {
        private readonly SessionRecording _recording;
        private readonly IObserversManager _observers;
        private readonly IDataSerializationFactory _writerFactory;
        private readonly Configuration.LocalSessionRecorderConfiguration _configuration;

        private readonly SerialDisposable _dataObserverSubscriber = new SerialDisposable();
        private readonly SerialDisposable _sessionEventsSubscriber = new SerialDisposable();
        private readonly SerialDisposable _schedulerDisposable = new SerialDisposable();

        private LocalSessionRecordingResult _result;
        private SessionRecordingPathsBuilder _paths;

        private bool _isRecording = false;

        public LocalSessionRecorder(SessionRecording recording, IObserversManager observers, IDataSerializationFactory writerFactory, ISessionsConfiguration configuration)
        {
            _recording = recording;
            _observers = observers;
            _writerFactory = writerFactory;
            _configuration = new Configuration.LocalSessionRecorderConfiguration(configuration);
        }


        public void Record()
        {
            if (_configuration.CreateSessionFolder)
            {
                _paths = new SessionRecordingPathsBuilder(_configuration.TargetPath, _recording);
            }
            else
            {
                _paths = new SessionRecordingPathsBuilder(_configuration.TargetPath);
            }

            _result = new LocalSessionRecordingResult(_recording, _paths.RootPath);

            _paths.Setup();

            AddConfigurations(_recording, _paths);

            var scheduler = new EventLoopScheduler();
            var deviceObserver = new LocalSessionDeviceRecorder(_recording, _writerFactory, _paths, _result, scheduler);

            _observers.Connect(deviceObserver);
            _dataObserverSubscriber.Disposable = Disposable.Create(() =>
            {
                _observers.Disconnect(deviceObserver);
            });

            string sessionEventsPath = _paths.BuildFilePath("session", _writerFactory.FileExtension);

            _sessionEventsSubscriber.Disposable = _recording.Events
                                                            .AttachWriter(sessionEventsPath, _writerFactory)
                                                            .Finally(Close)
                                                            .ObserveOn(scheduler: scheduler).Subscribe();
            _schedulerDisposable.Disposable = scheduler;

            _result.Paths.Add(sessionEventsPath);
            _recording.Results.Add(_result);

            _isRecording = true;
        }


        private static void AddConfigurations(SessionRecording recording, SessionRecordingPathsBuilder paths)
        {
            var selectedStreamingDevices = recording.DeviceConfigurations.Keys.Where(d => DeviceType.StreamingDevices.Contains(d)).ToList();
            foreach (var streamingDevice in selectedStreamingDevices)
            {
                string key = "TargetPath";
                string value = paths.BuildDeviceFilePath(streamingDevice, "data", "bin");

                recording.DeviceConfigurations.AddOrUpdate
                (
                    key: streamingDevice,
                    addValueFactory: d => new Dictionary<string, object>()
                    {
                        { key, value }
                    },
                    updateValueFactory: (_, configurations) =>
                    {
                        configurations.AddOrUpdate(key, value, (__, ___) => value);
                        return configurations;
                    }
                );
            }
        }


        private void Close()
        {
            _dataObserverSubscriber.Disposable = Disposable.Empty;
            _sessionEventsSubscriber.Disposable = Disposable.Empty;
            _schedulerDisposable.Disposable = Disposable.Empty;
            if (_isRecording)
            {
                _isRecording = false;

                SaveRecordingSettings();
                SaveRecordingDefinition();

                _result.Close();

                if (_recording.StartedAt.HasValue
                    && _recording.StartedAt.Value > DateTime.MinValue)
                {
                    //var result = new LocalSessionRecordingResult(_recording, _paths.RootPath, Paths);
                    //_recording.Results.Add(result);
                    Closed?.Invoke(this, _result);
                }
                else 
                {
                    _recording.Results.Remove(_result);
                    _result.Paths.Clear();
                    _paths.Clean();
                }
            }
        }


        private void SaveRecordingSettings()
        {
            var settings = _recording.Settings.DumpSettings();

            if (settings != null)
            {
                SaveData("settings", settings);
            }
        }

        private void SaveData(string name, object data)
        {
            try
            {
                string settingsPath = _paths.BuildFilePath(name, _writerFactory.FileExtension);

                using (var streamWriter = new StreamWriter(settingsPath, true, new UTF8Encoding(false)))
                {
                    using (var writer = _writerFactory.CreateWriterForType(streamWriter, data.GetType()))
                    {
                        writer.Write(data);
                        writer.Close();
                    }
                }

                _result.Paths.Add(settingsPath);
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }
        }

        private void SaveRecordingDefinition()
        {
            SaveData("definition", _recording.Definition);
        }


        public IConfiguration Configuration => _configuration;


        public IEnumerable<string> Paths => _paths.Paths;


        public event EventHandler<ISessionRecordingResult> Closed;


        #region IDisposable Members

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    Close();
                    _sessionEventsSubscriber.Dispose();
                    _dataObserverSubscriber.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
