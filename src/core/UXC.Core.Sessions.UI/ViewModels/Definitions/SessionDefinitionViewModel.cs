/**
 * UXC.Core.Sessions.UI
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
using UXC.Core.Devices;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels
{
    public class SessionDefinitionViewModel : BindableBase, ISessionChoiceViewModel
    {
        private readonly SessionDefinition _definition;

        public SessionDefinitionViewModel(SessionDefinition definition)
        {
            _definition = definition;
            SelectedDevices = definition.SelectedDeviceTypes.ToList();
        }

        public int Id => _definition.Id;

        public string Name { get { return _definition.Name; } set { } }

        public bool CanEditName => false;

        public string ChoiceName => $"{_definition.Project}: {_definition.Name}";

        public ICollection<DeviceType> SelectedDevices { get; }

        private bool canEditDevices = false;
        public bool CanEditDevices
        {
            get { return canEditDevices; }
            private set { Set(ref canEditDevices, value); }
        }

    //    public bool CanLockDevices => false;

        public virtual SessionDefinition GetDefinition()
        {
            var definition = _definition.Clone();

            var addedDevices = SelectedDevices.Where(d => _definition.SelectedDeviceTypes.Contains(d) == false)
                                              .Select(d => new SessionDeviceDefinition(d));

            definition.Devices = _definition.Devices
                                            .Where(d => SelectedDevices.Contains(d.Device))
                                            .Concat(addedDevices)
                                            .ToList();

            return definition;
        }
    }
}
