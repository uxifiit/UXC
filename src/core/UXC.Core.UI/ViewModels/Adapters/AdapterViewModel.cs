/**
 * UXC.Core.UI
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UXC.Devices.Adapters;
using UXC.Core.Common.Commands;
using UXC.Core.Devices;
using UXC.Core.Devices.Calibration;
using UXC.Core.ViewServices;
using System.Collections.ObjectModel;
using UXC.Devices.Adapters.Commands;
using System.Threading;
using UXI.Common.Helpers;
using System.Windows.Threading;
using UXI.Common.Extensions;

namespace UXC.Core.ViewModels.Adapters
{
    public class AdapterViewModel : ProgressViewModel
    {
        private readonly IDeviceAdapter _adapter;

        public AdapterViewModel(IDeviceAdapter adapter)
        {
            _adapter = adapter;

            _adapter.Observables.States.Subscribe(s => State = s.State);
            _adapter.Observables.States.Subscribe(s => UpdateCommands());

            _adapter.CommandExecutionChanged += (_, args) =>
            {
                if (args.IsWorking)
                {
                    IsLoading = true;
                    args.Completed += (__, r) => IsLoading = false; // TODO unregister handler
                }
                else
                {
                    IsLoading = false;
                }
            };

            State = _adapter.State;
        }


        public IDeviceAdapter Adapter => _adapter;


        private void UpdateCommands()
        {
            NextStates.Clear();

            foreach (var state in _adapter.AvailableForwardActions)
            {
                NextStates.Add(state);
            }

            foreach (var state in _adapter.AvailableBackwardActions)
            {
                NextStates.Add(state);
            }

            executeActionCommand?.RaiseCanExecuteChanged();
        }


        public ObservableCollection<DeviceAction> NextStates { get; } = new ObservableCollection<DeviceAction>();



        private RelayCommand<DeviceAction> executeActionCommand;
        public RelayCommand<DeviceAction> ExecuteActionCommand => executeActionCommand
            ?? (executeActionCommand = new RelayCommand<DeviceAction>
            (
                action => _adapter.ExecuteActionAsync(action, CancellationToken.None).Forget(),
                action => _adapter.CanExecuteAction(action)
            ));                                             



        private DeviceState state;
        public DeviceState State
        {
            get { return state; }
            private set
            {
                Set(ref state, value);
            }
        }
    }
}
