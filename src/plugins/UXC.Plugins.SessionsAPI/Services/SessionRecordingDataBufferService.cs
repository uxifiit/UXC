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
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Plugins.SessionsAPI.Entities;
using UXC.Plugins.SessionsAPI.Recording;

namespace UXC.Plugins.SessionsAPI.Services
{
    public class SessionRecordingDataBufferService
    {
        private readonly InMemoryRecordingDataSource _dataSource;

        public SessionRecordingDataBufferService(InMemoryRecordingDataSource dataSource)
        {
            _dataSource = dataSource;
        }


        public bool TryGetData(string sessionId, string deviceTypeCode, out DeviceData data, DateTime? from = null)
        {
            InMemoryRecordingDataAccessBuffer buffer;

            data = null;

            return _dataSource.TryGetBuffer(sessionId, out buffer)
                && TryGetData(buffer, deviceTypeCode, out data, from);
        }


        public bool TryGetData(string deviceTypeCode, out DeviceData data, DateTime? from = null)
        {
            InMemoryRecordingDataAccessBuffer buffer = _dataSource.Current;

            data = null;

            return buffer != null
                && TryGetData(buffer, deviceTypeCode, out data, from);
        }


        private static bool TryGetData(InMemoryRecordingDataAccessBuffer buffer, string deviceTypeCode, out DeviceData data, DateTime? from = null)
        {               
            DeviceType device;

            data = null;

            return DeviceType.TryResolveExistingType(deviceTypeCode, out device)
                && buffer.TryGetNextPart(device, from ?? DateTime.MinValue, out data);
        }


        public IEnumerable<string> Recordings => _dataSource.GetContainedRecordings();


        public string CurrentRecordingId => _dataSource.CurrentId;


        //public void Remove(string sessionId)
        //{
        //    if (String.IsNullOrWhiteSpace(sessionId) == false)
        //    {
        //        _dataSource.Close(sessionId);
        //    }
        //}
    }
}
