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
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core;
using UXC.Core.Common.Events;
using UXC.Core.Devices;
using UXC.Plugins.SessionsAPI.Recording;
using UXC.Sessions;
using UXC.Sessions.Recording.Local;
using UXI.Common.Extensions;

namespace UXC.Plugins.SessionsAPI.Services
{
    public class SessionRecordingResultsControlService : NotifyStateChangedBase<ControlServiceState>, IControlService
    {
        private readonly SessionRecordingResults _results;
        private readonly ISessionsControl _sessions;
        private IDisposable _resultsSubscription;

        public SessionRecordingResultsControlService(ISessionsControl sessions, SessionRecordingResults results)
        {
            _results = results;
            _sessions = sessions;
        }


        public bool AutoStart => true;


        public void Start()
        {
            var _resultsSubscription = _sessions.CompletedRecordings
                     .OfType<LocalSessionRecordingResult>()
                     .Where(result => result.Paths.Any())
                     .Subscribe(result => _results.Add(ApiRoutes.Recording.ConvertToRouteString(result.Recording.OpenedAt), result));

            State = ControlServiceState.Running;
        }


        public void Stop()
        {
            var resultsSubscription = ObjectEx.GetAndReplace(ref _resultsSubscription, null);
            resultsSubscription?.Dispose();

            State = ControlServiceState.Stopped;
        }


        public bool IsWorking() => false;
    }
}
