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
using GalaSoft.MvvmLight.CommandWpf;
using UXC.Core.Modules;

namespace UXC.Core.ViewModels
{
    public class SettingsViewModel
    {
        public SettingsViewModel(IEnumerable<ISettingsSectionViewModel> sections, IModulesService modules)
        {
            AddSections(sections);
            modules.Register<ISettingsSectionViewModel>(this, AddSections);
        }


        public ObservableCollection<ISettingsSectionViewModel> Sections { get; } = new ObservableCollection<ISettingsSectionViewModel>();


        private void AddSections(IEnumerable<ISettingsSectionViewModel> sections)
        {
            sections?.Where(s => Sections.Contains(s) == false)
                     .Execute(s => Sections.Add(s))
                     .ForEach(s => s.Reload());
        }


        private RelayCommand saveCommand;
        public RelayCommand SaveCommand => saveCommand
            ?? (saveCommand = new RelayCommand(() => Sections.ForEach(s => s.Save())));


        private RelayCommand reloadCommand;
        public RelayCommand ReloadCommand => reloadCommand
            ?? (reloadCommand = new RelayCommand(() => Sections.ForEach(s => s.Reload())));
    }
}
