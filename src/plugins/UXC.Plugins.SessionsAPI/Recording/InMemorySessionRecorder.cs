/**
 * UXC.Plugins.SessionsAPI
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Configuration;
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Core.Data.Serialization;
using UXC.Core.Managers;
using UXC.Devices.Adapters;
using UXC.Observers;
using UXC.Sessions;
using UXC.Sessions.Recording;
using UXI.Common;
using UXI.Common.Extensions;

namespace UXC.Plugins.SessionsAPI.Recording
{
    class InMemorySessionRecorder : DisposableBase, ISessionRecorder
    {
        private readonly SessionRecording _recording;
        private readonly IObserversManager _observers;
        private readonly InMemoryRecordingDataSource _dataSource;

        private readonly Configuration.BufferSessionRecorderConfiguration _configuration;

        private IDeviceObserver _deviceObserver;

        public InMemorySessionRecorder(SessionRecording recording, IObserversManager observers, InMemoryRecordingDataSource dataSource)
        {
            _recording = recording;
            _observers = observers;
            _dataSource = dataSource;
            _configuration = new Configuration.BufferSessionRecorderConfiguration();
        }


        public event EventHandler<ISessionRecordingResult> Closed { add { } remove { } }


        public void Record()
        {
            var buffer = _dataSource.Open(_recording, ApiRoutes.Recording.ConvertToRouteString(_recording.OpenedAt));
            _deviceObserver = new InMemorySessionDeviceRecorder(_recording, buffer, _configuration);

            _observers.Connect(_deviceObserver);
        }


        private void Close()
        {
            ObjectEx.GetAndReplace(ref _deviceObserver, null);
            if (_deviceObserver != null)
            {
                _observers.Disconnect(_deviceObserver);
            }
        }


        public IConfiguration Configuration => _configuration;


        #region IDisposable Members

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    Close();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
