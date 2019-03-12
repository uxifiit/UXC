/**
 * UXC.Core
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
using System.Threading;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Models;
using UXI.Common.Helpers;
using UXC.Observers;
using UXC.Core.Managers;
using UXC.Core.Devices;
using UXC.Devices.Adapters.Commands;
using UXC.Models.Contexts;
using UXC.Core.Logging;
using System.Reactive.Disposables;

namespace UXC.Core.Managers.Adapters
{
    public class AdaptersControl : IAdaptersControl
    {
        private const int MAX_ATTEMPT_LIMIT = 3;
        private static readonly TimeSpan NEXT_ATTEMPT_DELAY = TimeSpan.FromSeconds(5);

        private readonly IAdaptersManager _manager;
        private readonly IDevicesContext _context;
        private readonly ILogger _logger;

        private readonly DeviceStateAttempts _attempts = new DeviceStateAttempts();
        private readonly SerialDisposable _tasks = new SerialDisposable();
        private readonly object _tasksLock = new object();

        public AdaptersControl(IAdaptersManager manager, IDevicesContext context, ILogger logger)
        {
            _logger = logger;
            _manager = manager;
            _context = context;
            _context.DeviceUpdated += async (_, args) => await CheckDeviceAsync(args.DeviceType, args.State);
        }
    

        private Task SetStateAsync(DeviceState state, CancellationToken cancellationToken)
        {
            foreach (var device in _manager.Connections.Select(c => c.Code.DeviceType))
            {
                _logger.Info(LogHelper.Prepare($"Device: {device.Code}, State: {state}"));

                _attempts.SetAttempt(device, state);
            }

            return ResolveDevicesAsync(GetDevicesToResolve(), cancellationToken);
        }


        private Task SetStateAsync(DeviceState state, IEnumerable<DeviceType> devices, CancellationToken cancellationToken)
        {
            foreach (var device in _manager.Connections.Select(c => c.Code.DeviceType))
            {
                if (devices.Contains(device))
                {
                    _logger.Info(LogHelper.Prepare($"Device: {device.Code}, State: {state}"));

                    _attempts.SetAttempt(device, state);
                }
                else
                {
                    _attempts.DisableAttempt(device);
                }
            }

            return ResolveDevicesAsync(GetDevicesToResolve(devices), cancellationToken);
        }


        private async Task ResolveDevicesAsync(IEnumerable<IDeviceAdapter> adapters, CancellationToken cancellationToken, bool overrideExisting = true)
        {
            IEnumerable<DeviceStateAttemptResult> results = null;
            CancellationDisposable cancellation = null;

            var adaptersToResolve = adapters.ToList();
            int attempt = 0;

            if (adapters.Any() == false)
            {
                return;
            }

            lock (_tasksLock)
            {
                ICancelable previous = (ICancelable)_tasks.Disposable;
                if (overrideExisting || previous == null || previous.IsDisposed)
                {
                    cancellation = new CancellationDisposable(CancellationTokenSource.CreateLinkedTokenSource(cancellationToken));
                    _tasks.Disposable = cancellation;
                }
                else
                {
                    return;
                }
            }

            using (cancellation)
            {
                while (cancellation.Token.IsCancellationRequested == false
                        && adaptersToResolve.Any()
                        && attempt++ <= MAX_ATTEMPT_LIMIT)
                {
                    if (attempt > 1)
                    {
                        await Task.Delay(NEXT_ATTEMPT_DELAY);
                    }

                    results = await ResolveAsync(adaptersToResolve, cancellation.Token);

                    adaptersToResolve = adaptersToResolve
                        .Where(a => results.Any(r => r.Status.DeviceType.Equals(a.Code.DeviceType)) == false
                                    || results.First(r => r.Status.DeviceType.Equals(a.Code.DeviceType)).Result == CommandResult.Failed)
                        .ToList();
                }
            }
        }


        private IList<IDeviceAdapter> GetDevicesToResolve(IEnumerable<DeviceType> devices = null)
        {
            var adapters = _manager.Connections;

            if (devices != null)
            {
                adapters = adapters.Where(d => devices.Contains(d.Code.DeviceType));
            }

            return adapters.OrderBy(d => d.Code.ConnectionType)
                           .Where(a => _attempts.CanAttemptCommand(a.Code.DeviceType, a.State))
                           .ToList();
        }

    
        private async Task<IEnumerable<DeviceStateAttemptResult>> ResolveAsync(IEnumerable<IDeviceAdapter> devices, CancellationToken token)
        {
            List<DeviceStateAttemptResult> results = new List<DeviceStateAttemptResult>();

            try
            {
                foreach (var device in devices)
                {
                    token.ThrowIfCancellationRequested();

                    DeviceStateAttempt attempt = null;
                    if (_attempts.CanAttemptCommand(device.Code.DeviceType, device.State, out attempt)
                        && device.RecentCommandExecution.IsWorking == false)
                    {
                        _logger.Info(LogHelper.Prepare($"Device: {device.Code.DeviceType.Code}, State: {attempt.TargetState}, Attempts: {attempt.Attempts}"));

                        DeviceStateAttemptResult result = await AttemptDeviceToStateAsync(device, attempt.TargetState, token);

                        attempt.AddResult(result.Result);

                        results.Add(result);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.Info(LogHelper.Prepare("Cancelled"));
            }

            return results;
        }


        private static async Task<DeviceStateAttemptResult> AttemptDeviceToStateAsync(IDeviceAdapter device, DeviceState targetState, CancellationToken cancellationToken)
        {
            CommandResult result = CommandResult.NotApplied;

            try
            {
                Task<CommandResult> execution = Task.FromResult(CommandResult.NotApplied);
                if (device.TryGoToState(targetState, cancellationToken, out execution))
                {
                    result = await execution;
                }
            }
            catch (Exception)
            {
                result = CommandResult.Failed;
            }

            return new DeviceStateAttemptResult(device.Code.DeviceType, targetState, DateTime.Now, result);
        }
      

        public Task DisconnectAsync(CancellationToken cancellationToken)
            => SetStateAsync(DeviceState.Disconnected, cancellationToken);


        public Task ConnectAsync(IEnumerable<DeviceType> devices, CancellationToken cancellationToken) 
            => SetStateAsync(DeviceState.Connected, devices, cancellationToken);


        public Task ConnectAsync(CancellationToken cancellationToken)
            => SetStateAsync(DeviceState.Connected, cancellationToken);


        public Task StopRecordingAsync(CancellationToken cancellationToken)
            => SetStateAsync(DeviceState.Connected, cancellationToken);


        public Task StartRecordingAsync(CancellationToken cancellationToken)
            => SetStateAsync(DeviceState.Recording, cancellationToken);


        public Task StartRecordingAsync(IEnumerable<DeviceType> devices, CancellationToken cancellationToken) 
            => SetStateAsync(DeviceState.Recording, devices, cancellationToken);


        public bool CheckAreDevicesInState(DeviceState state)
        {
            return _manager.Connections
                           .All(d => d.State == state);
        }


        public bool CheckAreDevicesInState(DeviceState state, IEnumerable<DeviceType> devices)
        {
            return _manager.Connections
                           .Where(d => devices.Contains(d.Code.DeviceType))
                           .All(d => d.State == state);
        }


        public void ResetConfigurations()
        {
            _manager.Connections.Select(c => c.Configurator).ForEach(c => c.ResetAll());
        }


        public void Configure(IDictionary<DeviceType, IDictionary<string, object>> configurations)
        {
            if (configurations != null && configurations.Any())
            {
                foreach (var device in _manager.Connections)
                {
                    if (configurations.ContainsKey(device.Code.DeviceType)
                        && device.Configurator.CanConfigure())
                    {
                        device.Configurator.Configure(configurations[device.Code.DeviceType]);
                    }
                }
            }
        }


        private async Task CheckDeviceAsync(DeviceType device, DeviceState state)
        {
            if (_attempts.CanAttemptCommand(device, state))
            {
                try
                {
                    var devicesToResolve = GetDevicesToResolve();

                    if (devicesToResolve.Any())
                    {
                        await ResolveDevicesAsync(devicesToResolve, CancellationToken.None, overrideExisting: false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(LogHelper.Prepare(), ex);
                }
            }
        }
    }
}
