/**
 * UXC.Core.Design
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
using Ninject.Modules;
using UXC.Core.Devices;
using UXC.Design.Devices;
using UXC.Devices.Adapters;

namespace UXC.Design
{
    public class DesignDevicesModule : NinjectModule
    {
        private readonly List<DeviceType> _devices;

        public DesignDevicesModule(params DeviceType[] devices)
        {
            _devices = devices.ToList() ?? new List<DeviceType>();
        }

        public DesignDevicesModule()
            : this
            (
                DeviceType.Physiological.EYETRACKER,
                DeviceType.Input.KEYBOARD,
                DeviceType.Input.MOUSE,
                DeviceType.Streaming.SCREENCAST,
                DeviceType.Streaming.WEBCAM_VIDEO
            )
        { }

        public override void Load()
        {
            var devices = _devices.Select(d => new DesignDevice(d));

            foreach (var adapter in devices)
            {
                Bind<IDevice>().ToConstant(adapter).InSingletonScope();
            }
        }
    }
}
