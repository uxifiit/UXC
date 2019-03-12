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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using UXC.Common;
using UXI.Common.UI;
using UXC.Core.Managers.Adapters;
using UXC.Core.Managers;
using UXC.Devices.Adapters;
using UXC.Models;
using UXC.Core.ViewServices;
using UXC.Core.Devices;
using UXC.Core.ViewModels.Adapters;
using UXI.Common;
using UXC.Core.ViewModels;

namespace UXC.ViewModels
{
    public class AdaptersViewModel : BindableBase
    {
        private readonly IAdaptersManager _manager;
        private readonly ViewModelResolver _resolver;

        public AdaptersViewModel(IAdaptersManager manager, ViewModelResolver resolver)
        {
            _resolver = resolver;

            _manager = manager;
            _manager.ConnectionsChanged += manager_ConnectionsChanged;

            Adapters = _manager.Connections
                               .Select(_resolver.Create)
                               .OfType<AdapterViewModel>()
                               .ToObservableCollection();
        }


        public ObservableCollection<AdapterViewModel> Adapters { get; }


        void manager_ConnectionsChanged(object sender, CollectionChangedEventArgs<IDeviceAdapter> e)
        {
            if (e.AddedItems.Any())
            {
                e.AddedItems
                 .Select(_resolver.Create)
                 .OfType<AdapterViewModel>()
                 .ForEach(a => Adapters.Add(a));
            }

            if (e.RemovedItems.Any())
            {
                Adapters.Where(a => e.RemovedItems.Contains(a.Adapter))
                        .ToList()
                        .ForEach(a => Adapters.Remove(a));
            }
        }
    }
}
