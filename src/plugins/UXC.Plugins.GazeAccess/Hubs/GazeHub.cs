/**
 * UXC.Plugins.GazeAccess
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using System.Reactive.Linq;
using UXC.Devices.Adapters;
using UXC.Observers;
using System.Reactive.Disposables;
using UXC.Plugins.GazeAccess.Observers;

namespace UXC.Plugins.GazeAccess.Hubs
{
    // TODO 13/09/2016 clients do not receive GazeData
    public class GazeHub : Hub
    {
        private readonly EyeTrackerObserver _observer;
        private readonly HubClients _clients;

        private static readonly ConcurrentDictionary<int, IDisposable> subscriptions = new ConcurrentDictionary<int, IDisposable>();
        private static readonly SerialDisposable stateSubscription = new SerialDisposable();

        private const int DEFAULT_SAMPLING = 0;


        public GazeHub(EyeTrackerObserver eyeTracker, HubClients clients)
        {
            _clients = clients;
            _observer = eyeTracker;

            _clients.GroupCreated += OnGroupCreated;
            _clients.GroupClosed += OnGroupClosed;

            _clients.FirstClientConnected += OnFirstClientConnected;
            _clients.LastClientDisconnected += OnLastClientDisconnected;
        }


        private static string GetGroupName(int group)
        {
            return group.ToString();
        }


        private void OnGroupCreated(object sender, int group)
        {
            //var context = GlobalHost.ConnectionManager.GetHubContext<GazeHub>(); // MK: hub clients do not receive RPC calls if this context is used 
            GazeHub hub = this; // instead context, we use this gazehub instance to access clients
            int framerate = group;
            string groupName = GetGroupName(group);

            if (subscriptions.ContainsKey(group) == false)
            {
                var gazeData = _observer.Data;

                if (framerate != DEFAULT_SAMPLING && framerate > 0)
                {
                    gazeData = gazeData.Sample(TimeSpan.FromMilliseconds(1000d / framerate));
                }

                var clientsGroup = hub.Clients.Group(groupName);

                IDisposable subscription = gazeData.Subscribe(g => clientsGroup.OnGazeData(g)); 
                subscriptions.TryAdd(group, subscription);
            }
        }


        private void OnLastClientDisconnected(object sender, EventArgs e)
        {
            stateSubscription.Disposable = Disposable.Empty;
        }


        private void OnFirstClientConnected(object sender, EventArgs e)
        {
            GazeHub hub = this;

            stateSubscription.Disposable = _observer.States.Subscribe(s => hub.Clients.All.OnStateChanged(s));
        }


        private void OnGroupClosed(object sender, int group)
        {
            IDisposable subscription;
            if (subscriptions.TryRemove(group, out subscription))
            {
                subscription.Dispose();
            }   
        }


        public async override Task OnConnected()
        {
            string client = Context.ConnectionId;

            int sampling = DEFAULT_SAMPLING;

            _clients.Add(client, sampling);

            await Groups.Add(client, GetGroupName(sampling));

            await base.OnConnected();
        }


        public async override Task OnDisconnected(bool stopCalled)
        {
            string client = Context.ConnectionId;

            int sampling = _clients.Remove(client);

            // await Groups.Remove(client, GetGroupName(sampling)); // MK: never completes, SignalR should handle disconnection by itself

            await base.OnDisconnected(stopCalled);
        }


        public async Task<bool> SetSampling(int sampling)
        {
            string client = Context.ConnectionId;

            if (sampling < 0)
            {
                return false;
            }

            int old = _clients.Update(client, sampling);

            await Groups.Remove(client, GetGroupName(old));
            await Groups.Add(client, GetGroupName(sampling));
            return true;
        }


        public int GetSampling()
        {
            string client = Context.ConnectionId;

            int group;
            return _clients.TryGet(client, out group) ? group : DEFAULT_SAMPLING;
        }


        public string CurrentState()
        {
            return _observer.RecentState.ToString();
        }
    }
}
