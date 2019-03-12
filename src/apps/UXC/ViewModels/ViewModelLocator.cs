/**
 * UXC
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
using GalaSoft.MvvmLight;
using Ninject;
using UXC.ViewModels;
using UXC.Core.ViewModels;
using UXC.Core.ViewServices;
using UXC.Modules;
using UXC.Core.ViewModels.Adapters;
using UXC.Sessions.ViewModels;
using UXI.Common.UI;
using System.Windows;
using UXC.Plugins.UXR.ViewModels;
using UXC.Plugins.UXR.ViewModels.Uploads;
using UXC.Core.ViewModels.Services;
using UXC.Core;

namespace UXC.ViewModels
{
    class ViewModelLocator
    {
        private readonly IKernel _kernel;

        public ViewModelLocator()
        {
#if DEBUG
            if (DesignTimeHelper.IsDesignTime)
            {
                _kernel = UXC.App.CreateKernel();
                LoadDesign();
            }
            else
            {
#endif
                _kernel = (Application.Current as App).Kernel;
#if DEBUG
            }
#endif
        }

        private void LoadDesign()
        {
            var app = _kernel.Get<IAppService>();
            app.Load();
            app.Start();
        }

        public AppViewModel App => _kernel.Get<AppViewModel>();

        public AboutViewModel About => _kernel.Get<AboutViewModel>();

        public AdaptersViewModel Adapters => _kernel.Get<AdaptersViewModel>();

        public SettingsViewModel Settings => _kernel.Get<SettingsViewModel>();

        public SessionsViewModel Sessions => _kernel.Get<SessionsViewModel>();

        public UXRNodeViewModel UXRNode => _kernel.Get<UXRNodeViewModel>();

        public SessionRecordingsDataViewModel SessionRecordingsData => _kernel.Get<SessionRecordingsDataViewModel>();

        public UploadsViewModel UXRUploads => _kernel.Get<UploadsViewModel>();

        public UploaderViewModel UXRUploader => _kernel.Get<UploaderViewModel>();

        public ControlServicesIconsViewModel Services => _kernel.Get<ControlServicesIconsViewModel>();
    }
}
