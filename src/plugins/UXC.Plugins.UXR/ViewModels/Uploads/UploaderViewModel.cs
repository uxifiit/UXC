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
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.CommandWpf;
using UXC.Plugins.UXR.Models.Uploads;
using UXI.Common.UI;

namespace UXC.Plugins.UXR.ViewModels.Uploads
{
    public class UploaderViewModel : BindableBase
    {
        public enum State
        {
            Disabled,
            Enabled,
            Working
        }

        private readonly IUploader _uploader;
        private readonly Dispatcher _dispatcher;

        public UploaderViewModel(IUploader uploader, Dispatcher dispatcher)
        {
            _uploader = uploader;
            _dispatcher = dispatcher;

            _uploader.CurrentUploadChanged += (_, upload) =>
            {
                _dispatcher.Invoke(() =>
                {
                    UpdateCurrentUpload(upload);
                });
            };

            _uploader.IsEnabledChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(IsEnabled));
                OnPropertyChanged(nameof(IsWorking));
                OnPropertyChanged(nameof(CurrentState));
                startCommand?.RaiseCanExecuteChanged();
                restartCommand?.RaiseCanExecuteChanged();
                stopCommand?.RaiseCanExecuteChanged();
            };

            _uploader.IsWorkingChanged += (_, __) =>
            {
                _dispatcher.Invoke(() =>
                {
                    OnPropertyChanged(nameof(IsWorking));
                    OnPropertyChanged(nameof(CurrentState));
                });
            };

            UpdateCurrentUpload(_uploader.CurrentUpload);
        }


        private void UpdateCurrentUpload(Upload upload)
        {
            CurrentUpload = upload != null ? new UploadViewModel(upload, _dispatcher) : null;
        }


        private UploadViewModel currentUpload;
        public UploadViewModel CurrentUpload
        {
            get { return currentUpload; }
            private set { Set(ref currentUpload, value); }
        }


        public bool IsEnabled => _uploader.IsEnabled;


        public bool IsWorking => _uploader.IsWorking;


        public State CurrentState => IsEnabled ? (IsWorking ? State.Working : State.Enabled) : State.Disabled;


        private RelayCommand startCommand;
        public RelayCommand StartCommand => startCommand
            ?? (startCommand = new RelayCommand(_uploader.Start, () => IsEnabled == false));


        private RelayCommand restartCommand;
        public RelayCommand RestartCommand => restartCommand
            ?? (restartCommand = new RelayCommand(_uploader.Start, () => IsEnabled && IsWorking == false));


        private RelayCommand stopCommand;
        public RelayCommand StopCommand => stopCommand
            ?? (stopCommand = new RelayCommand(_uploader.Stop, () => IsEnabled));
    }
}
