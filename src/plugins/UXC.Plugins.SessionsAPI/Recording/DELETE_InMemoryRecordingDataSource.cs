///**
// * UXC.Plugins.SessionsAPI
// * Copyright (c) 2018 The UXC Authors
// * 
// * Licensed under GNU General Public License 3.0 only.
// * Some rights reserved. See COPYING, AUTHORS.
// *
// * SPDX-License-Identifier: GPL-3.0-only
// */
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UXC.Sessions;

//namespace UXC.Plugins.SessionsAPI.Recording
//{
//    public class InMemoryRecordingDataSource
//    {
//        private readonly ConcurrentDictionary<string, InMemoryRecordingDataAccessBuffer> _buffers = new ConcurrentDictionary<string, InMemoryRecordingDataAccessBuffer>();

//        public InMemoryRecordingDataAccessBuffer Open(SessionRecording recording, string sessionId)
//        {
//            CurrentId = sessionId;
//            return Current = _buffers.GetOrAdd(sessionId, _ => new InMemoryRecordingDataAccessBuffer(recording.SelectedDevices));
//        }


//        public ICollection<string> GetContainedRecordings()
//        {
//            return _buffers.Keys;
//        }


//        public bool TryGetBuffer(string sessionId, out InMemoryRecordingDataAccessBuffer buffer)
//        {
//            return _buffers.TryGetValue(sessionId, out buffer);
//        }


//        public InMemoryRecordingDataAccessBuffer Current { get; private set; }

//        public string CurrentId { get; private set; }


//        public void Close(string sessionId)
//        {
//            InMemoryRecordingDataAccessBuffer buffer;
//            if (_buffers.TryRemove(sessionId, out buffer))
//            {
//                buffer.Close();

//                if (Current == buffer)
//                {
//                    Current = null;
//                    CurrentId = null;
//                }
//            }
//        }
//    }
//}
