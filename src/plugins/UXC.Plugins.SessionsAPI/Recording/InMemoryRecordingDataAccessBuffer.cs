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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXC.Core.Data;

namespace UXC.Plugins.SessionsAPI.Recording
{
    public class InMemoryRecordingDataAccessBuffer
    {
        private readonly Dictionary<DeviceType, ConcurrentQueue<DeviceData>> _recordingDataParts;

        public InMemoryRecordingDataAccessBuffer(IEnumerable<DeviceType> devices)
        {
            _recordingDataParts = new Dictionary<DeviceType, ConcurrentQueue<DeviceData>>();
            devices?.ForEach(d => _recordingDataParts.Add(d, new ConcurrentQueue<DeviceData>()));
        }


        public void Add(DeviceType device, DeviceData data)
        {
            var parts = _recordingDataParts[device];

            parts.Enqueue(data);
        }


        public bool TryGetNextPart(DeviceType device, DateTime fromTimeStamp, out DeviceData nextData)
        {
            var parts = _recordingDataParts[device];
            DeviceData data;

            if (parts.Any())
            {
                while (parts.TryPeek(out data) && data.Timestamp <= fromTimeStamp)
                {
                    parts.TryDequeue(out data);
                }

                if (parts.TryPeek(out data))
                {
                    nextData = data;
                    return true;
                }
            }

            nextData = null;
            return false;
        }


        public bool HasData(DeviceType device)
        {
            ConcurrentQueue<DeviceData> queue;
            return _recordingDataParts.TryGetValue(device, out queue)
                && queue.Any();
        }


        public void Close()
        {
            var queues = _recordingDataParts.Values.ToList();
            _recordingDataParts.Clear();
        }
    }
}
