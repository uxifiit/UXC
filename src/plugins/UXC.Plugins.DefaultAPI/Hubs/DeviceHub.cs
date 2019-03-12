/**
 * UXC.Plugins.DefaultAPI
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
using Microsoft.AspNet.SignalR;
using UXC.Plugins.DefaultAPI.Entities;
using UXC.Plugins.DefaultAPI.Services;

namespace UXC.Plugins.DefaultAPI.Hubs
{
    public class DeviceHub : Hub
    {
        private readonly DeviceService _service;

        public DeviceHub(DeviceService service)
        {
            _service = service;
        }


        public List<DeviceStatusInfo> GetList()
        {
            return _service.GetList();
        }


        public DeviceStatusInfo GetDetails(string deviceTypeCode)
        {
            try
            {
                return _service.GetDetails(deviceTypeCode);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
