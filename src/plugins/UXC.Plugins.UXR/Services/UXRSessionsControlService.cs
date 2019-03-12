/**
 * UXC.Plugins.UXR
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
using System.Reactive.Linq;
using UXC.Core;
using UXC.Plugins.UXR.Models;
using UXC.Sessions;
using UXR.Studies.Client;
using UXI.Common.Extensions;
using UXI.Common.Helpers;
using System.Diagnostics;
using System.IO;
using UXC.Sessions.Recording.Local;
using System.Reactive.Disposables;
using UXC.Plugins.UXR.Models.Uploads;
using UXC.Core.Common.Events;
using System.Threading;

namespace UXC.Plugins.UXR.Services
{
    internal class UXRSessionsControlService : NotifyStateChangedBase<ControlServiceState>, IControlService
    {
        private readonly UXRSessionDefinitionsSource _definitions;
        private readonly UXRNodeService _uxrNode;

        private IDisposable _subscription;

        public bool AutoStart => true;

        internal UXRSessionsControlService(UXRSessionDefinitionsSource definitions, UXRNodeService uxrNode)
        {
            _definitions = definitions;
            _uxrNode = uxrNode;
        }


        public void Start()
        {
            // could have used SelectMany, but that would terminate the sequence with first error in any of the SessionRecordings
            // BUT events do not throw errors, so use it


            //var lastEvents = _sessions.Recordings.SelectMany(r => r.Events.LastOrDefaultAsync());

            //var lastEvents = Observable.Create<SessionRecordingEvent>(o =>
            //{
            //    return _sessions.Recordings
            //                    .Subscribe(r =>
            //                    {
            //                        r.Events
            //                         .LastOrDefaultAsync()
            //                         .Subscribe(ev => o.OnNext(ev));
            //                    });
            //                    //    r.Events
            //                    //     .Materialize()
            //                    //     .Buffer(2, 1)
            //                    //     .Where(notifications => notifications.Any(n => n.Kind == System.Reactive.NotificationKind.OnCompleted))
            //                    //     .Select(notifications => notifications.FirstOrDefault(n => n.Kind == System.Reactive.NotificationKind.OnNext)?.Value)
            //                    //     .Where(e => e != null)
            //                    //     .Subscribe(ev => o.OnNext(ev));
            //                    //});
            //});

            var nodeUpdates = Observable.FromEventPattern<IUXRNodeContext>
            (
                handler => _uxrNode.NodeStatusUpdated += handler,
                handler => _uxrNode.NodeStatusUpdated -= handler    
            );

            _subscription = /*Observable.Merge(lastEvents.Cast<object>(), nodeUpdates.Cast<object>())*/
                                      nodeUpdates.Subscribe(_ => _definitions.RefreshAsync(CancellationToken.None).Forget());

            //lastEvents.Where(e => e.State == SessionState.Completed)
            //          .OfType<SessionCompletedEvent>()
            //          .Subscribe(SendData_And_DeleteIfKeepEqualsFalse);

            //nodeUpdates.Subscribe(_ => _uxrSessions.UpdateSessionDefinitionsAsync().Forget())


            ////Observable.Merge(nodeIdChanges, lastEvents.Select(_ => 0))
            //          .Subscribe(e => _uxrSessions.UpdateSessionDefinitionsAsync().Forget())

            //lastEvents.Where(e => e.State == SessionState.Cancelled)
            //          .OfType<SessionCancelledEvent>()
            //          .Subscribe(DeleteData);

            // check for sessions when there is not any running
            // show progress indicatior in the UI while checking
            // provide way to force recheck in the UI

            // tasks:
            // queue which will complete scheduled tasks
            // queue is serialized or can be restored from the data on the app start 
            // this service or some other class generates tasks
            // tasks may be aggregated - zip, copy, remove, send - or implemented by the module
            // Task, TaskScheduler, Queue, etc. are already reserved by framework classes


            // OR tasks are handled by separate application running in the background
            // so the UXC app does not have to sync the data with server
            // but then there's no need for this ControlService? 
            // - there might be: so this service contacts the background app
            // the service is WindowsService
            // possibly it may be run under the local account (not administrator), 
            //  and thus operated (started, stopped) by the app without the admin permissions
            //  -- will not work probably, because otherwise the service will be available only for the owning account
            //  -- but do we need it to be available for other accounts?
            //  -- the service may run for the local account only
            //  -- then it may be operated by the user himself -> usier for study conductors to fix problems
            // TODO check whether the app can control windows service without admin privileges if the service runs under the current user account 

            State = ControlServiceState.Running;
        }

     

        public void Stop()
        {
            var subscription = ObjectEx.GetAndReplace(ref _subscription, null);
            subscription?.Dispose();

            State = ControlServiceState.Stopped;
        }


        public bool IsWorking() => false;
    }
}
