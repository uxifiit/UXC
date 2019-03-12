/**
 * UXC.Devices.EyeTracker.Calibration.UI
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
using UXC.Core.Modules;
using UXC.Core.ViewModels;
using UXC.Devices.EyeTracker.Configuration;
using UXI.Common.UI;

namespace UXC.Devices.EyeTracker.ViewModels
{
    public class EyeTrackerSettingsSectionViewModel : BindableBase, ISettingsSectionViewModel
    {
        private readonly IEyeTrackerConfiguration _configuration;

        public EyeTrackerSettingsSectionViewModel(IEyeTrackerConfiguration configuration, IEnumerable<ITrackerFinder> finders, IModulesService modules)
        {
            _configuration = configuration;

            AddDrivers(finders);
            modules.Register<ITrackerFinder>(this, AddDrivers);
        }


        private void AddDrivers(IEnumerable<ITrackerFinder> finders)
        {
            if (finders != null && finders.Any())
            {
                finders.Select(f => f.Name)
                       .Where(f => Drivers.Contains(f) == false)
                       .ForEach(f => Drivers.Add(f));
            }
        }


        public void Save()
        {
            _configuration.SelectedDriver = SelectedDriver ?? String.Empty;
        }


        public void Reload()
        {
            SelectedDriver = _configuration.SelectedDriver?.Trim() ?? String.Empty;
        }


        public ObservableCollection<string> Drivers { get; } = new ObservableCollection<string>();


        private string selectedDriver;
        public string SelectedDriver
        {
            get { return selectedDriver; }
            set { Set(ref selectedDriver, value); }
        }


        public string Name => "Eye Tracker";
    }
}
