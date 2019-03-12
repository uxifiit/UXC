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
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXI.Common.Extensions;
using UXC.Core.Devices;
using UXC.Devices.Adapters.Commands;

namespace UXC.Core.Managers.Adapters
{
    internal class DeviceStateAttempts
    {
        private readonly ConcurrentDictionary<DeviceType, DeviceStateAttempt> _attempts = new ConcurrentDictionary<DeviceType, DeviceStateAttempt>();

        public DeviceStateAttempt SetAttempt(DeviceType device, DeviceState state)
        {
            DeviceStateAttempt attempt;
            if (_attempts.TryGetValue(device, out attempt))
            {
                attempt.TargetState = state;
            }
            else
            {
                attempt = new DeviceStateAttempt(state);
                _attempts.TryAdd(device, attempt);
            }

            attempt.IsEnabled = true;

            return attempt;
        }


        public bool CanAttemptCommand(DeviceType device, DeviceState state, out DeviceStateAttempt attempt)
        {
            return _attempts.TryGetValue(device, out attempt)
                && attempt.IsEnabled
                && attempt.TargetState != state;
        }


        public bool CanAttemptCommand(DeviceType device, DeviceState state)
        {
            DeviceStateAttempt attempt;
            return CanAttemptCommand(device, state, out attempt);
        }


        public void DisableAttempt(DeviceType device)
        {
            DeviceStateAttempt attempt = null;

            if (_attempts.TryGetValue(device, out attempt))
            {
                attempt.IsEnabled = false;
            }
        }
    }


    internal class DeviceStateAttempt
    {
        private readonly List<CommandResult> _results = new List<CommandResult>();

        public DeviceStateAttempt(DeviceState state)
        {
            targetState = state;
        }


        public void AddResult(CommandResult result) => _results.Add(result);


        public void Reset() => _results.Clear();


        private DeviceState targetState;
        public DeviceState TargetState
        {
            get { return targetState; }
            set
            {
                if (targetState != value)
                {
                    targetState = value;
                    _results.Clear();
                }
            }
        }


        public int Attempts => _results.Count;


        public CommandResult LastResult => _results.Any() ? _results.Last() : CommandResult.NotApplied;


        public bool IsEnabled { get; set; }
    }
}
