/**
 * UXC.Core.UI
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using GalaSoft.MvvmLight.CommandWpf;
using UXC.Core.Common.Commands;
using UXC.Core.Modules;
using UXI.Common.Extensions;
using UXI.Common.UI;

namespace UXC.Core.ViewModels.Services
{
    public class ControlServiceViewModel : BindableBase
    {
        private readonly IControlService _service;
        private readonly Dispatcher _dispatcher;

        public ControlServiceViewModel(IControlService service, Dispatcher dispatcher) 
            : this(service, dispatcher, service.GetType().Name.Replace("ControlService", ""))
        {
            
        }


        public ControlServiceViewModel(IControlService service, Dispatcher dispatcher, string name)
        {
            Name = name;

            _dispatcher = dispatcher;

            _service = service;
            _service.StateChanged += service_StateChanged;
        }


        private void service_StateChanged(object sender, Common.Events.ValueChangedEventArgs<ControlServiceState> e)
        {
            _dispatcher.InvokeAsync(() =>
            {
                OnServiceStateChanged(e);
            }).Task.Forget();
        }


        protected virtual void OnServiceStateChanged(Common.Events.ValueChangedEventArgs<ControlServiceState> state)
        {
            startCommand?.RaiseCanExecuteChanged();
            stopCommand?.RaiseCanExecuteChanged();
            switchStartStopCommand?.RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(IsRunning));
            OnPropertyChanged(nameof(IsError));
            OnPropertyChanged(nameof(State));
        }


        public IControlService Service => _service;


        public virtual string Name { get; }


        public bool IsRunning => _service.State == ControlServiceState.Running;

        public bool IsError => _service.State == ControlServiceState.Error;


        public ControlServiceState State => _service.State;


        private RelayCommand startCommand;
        public RelayCommand StartCommand => startCommand
            ?? (startCommand = new RelayCommand(_service.Start, CanStart));

        private bool CanStart() => _service.State != ControlServiceState.Running;


        private RelayCommand stopCommand;
        public RelayCommand StopCommand => stopCommand
            ?? (stopCommand = new RelayCommand(_service.Stop, CanStop));

        private bool CanStop() => _service.State == ControlServiceState.Running;


        private RelayCommand switchStartStopCommand;
        public RelayCommand SwitchStartStopCommand => switchStartStopCommand
            ?? (switchStartStopCommand = new RelayCommand(SwitchStartStop));

        private void SwitchStartStop()
        {
            if (CanStart())
            {
                _service.Start();
            }
            else if (CanStop())
            {
                _service.Stop();
            }
        }
    }
}
