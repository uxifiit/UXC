/**
 * GazeVisualization
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
using System.Windows.Threading;
using GazeVisualization.Observers;
using GazeVisualization.ViewModels;
using GazeVisualization.Views;
using UXC.Core;
using UXC.Core.Common.Events;
using UXI.Common.Extensions;

namespace GazeVisualization.Services
{
    class GazeDisplayControlService : NotifyStateChangedBase<ControlServiceState>, IControlService
    {
        private readonly Dispatcher _dispatcher;
        private readonly GazeObserver _observer;

        private DisplayWindow _window;

        public GazeDisplayControlService(Dispatcher dispatcher, GazeObserver observer)
        {
            _dispatcher = dispatcher;
            _observer = observer;
        }


        public bool AutoStart => false;


        public bool IsRunning { get; private set; }

        public void Start()
        {
            _dispatcher.Invoke(OpenWindow);
        }


        private void OpenWindow()
        {
            _window = new DisplayWindow();
            _window.DataContext = new GazeViewModel(_observer);
            _window.Closed += Window_Closed;
            _window.Show();

            State = ControlServiceState.Running;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            State = ControlServiceState.Stopped;
        }

        public void Stop()
        {
            _dispatcher.Invoke(CloseWindow);
        }


        private void CloseWindow()
        {
            var window = ObjectEx.GetAndReplace(ref _window, null);
            window?.Close();
        }


        public bool IsWorking() => false;
    }
}
