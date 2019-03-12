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
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UXC.Core;
using UXC.Core.Common.Events;
using UXC.Plugins.UXR.Configuration;
using UXC.Plugins.UXR.Extensions;
using UXC.Plugins.UXR.Models;
using UXI.Common.Extensions;
using UXR.Studies.Client;

namespace UXC.Plugins.UXR.Services
{
    internal class UXRStatusUpdateControlService : NotifyStateChangedBase<ControlServiceState>, IControlService
    {
        //private static readonly TimeSpan UPDATE_INTERVAL = TimeSpan.FromSeconds(5);
        private readonly UXRNodeService _node;
        private readonly TimeSpan _updateInterval;

        private IDisposable _subscription;

        internal UXRStatusUpdateControlService(UXRNodeService node, IUXRConfiguration configuration)
        {
            _node = node;
            _updateInterval = TimeSpan.FromSeconds(configuration.StatusUpdateIntervalSeconds);
            // TODO add immediate update when node name gets changed
        }


        public bool AutoStart => true;


        public void Start()
        {
            var scheduler = NewThreadScheduler.Default;

            _subscription = Observable.FromAsync(_node.UpdateNodeStatusAsync, scheduler)
                                      .RepeatAfterDelay(_updateInterval, scheduler)
                                      .Subscribe();

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
