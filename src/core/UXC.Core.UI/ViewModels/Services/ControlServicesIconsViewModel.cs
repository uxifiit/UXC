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
using UXC.Core.Modules;

namespace UXC.Core.ViewModels.Services
{
    public class ControlServicesIconsViewModel
    {
        private readonly ViewModelResolver _resolver;

        public ControlServicesIconsViewModel(IEnumerable<IControlService> services, IModulesService modules, ViewModelResolver resolver)
        {
            _resolver = resolver;

            AddServices(services);
            modules.Register<IControlService>(this, AddServices);
        }


        private void AddServices(IEnumerable<IControlService> services)
        {
            services?.Where(s => Icons.Any(i => i.Service == s) == false)
                     .ToList()
                     .Where(s => _resolver.CanCreate(s))
                     .Select(s => (ControlServiceViewModel)_resolver.Create(s))
                     .ForEach(Icons.Add);
        }


        public ObservableCollection<ControlServiceViewModel> Icons { get; } = new ObservableCollection<ControlServiceViewModel>();
    }
}
