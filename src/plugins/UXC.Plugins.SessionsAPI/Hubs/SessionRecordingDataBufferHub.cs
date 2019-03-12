/**
 * UXC.Plugins.SessionsAPI
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
using UXC.Core.Data;
using UXC.Plugins.SessionsAPI.Services;

namespace UXC.Plugins.SessionsAPI.Hubs
{
    public class SessionRecordingDataBufferHub
    {
        private readonly SessionRecordingDataBufferService _service;

        public SessionRecordingDataBufferHub(SessionRecordingDataBufferService service)
        {
            _service = service;
        }

        public DeviceData GetRecordingData(string recording, string device, DateTime? from = null)
        {
            DeviceData data;
            return _service.TryGetData(recording, device, out data, from) ? data : null;
        }


        public DeviceData GetCurrentRecordingData(string device, DateTime? from = null)
        {
            DeviceData data;
            return _service.TryGetData(device, out data, from) ? data : null;
        }


        public List<string> GetRecordings()
        {
            return _service.Recordings.ToList();
        }


        public string GetCurrentRecordingId()
        {
            return _service.CurrentRecordingId;
        }
    }
}
