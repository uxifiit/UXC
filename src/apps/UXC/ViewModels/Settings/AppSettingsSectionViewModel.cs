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
using UXC.Configuration;
using UXC.Core.ViewModels;
using UXI.Common.UI;

namespace UXC.ViewModels.Settings
{
    public class AppSettingsSectionViewModel : BindableBase, ISettingsSectionViewModel
    {
        private readonly IAppConfiguration _appConfiguration;

        public AppSettingsSectionViewModel(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public string Name => "App";

        public void Reload()
        {
            HideOnClose = _appConfiguration.HideOnClose;
        }

        public void Save()
        {
            _appConfiguration.HideOnClose = HideOnClose;
        }


        private bool hideOnClose;
        public bool HideOnClose
        {
            get { return hideOnClose; }
            set { Set(ref hideOnClose, value); }
        }
    }
}
