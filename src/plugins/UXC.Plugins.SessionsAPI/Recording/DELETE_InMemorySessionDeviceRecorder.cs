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
//using System.Collections.Generic;
//using System.Linq;
//using System.Reactive.Disposables;
//using System.Reactive.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UXC.Core.Devices;
//using UXC.Core.Data;
//using UXC.Core.Data.Serialization;
//using UXC.Devices.Adapters;
//using UXC.Observers;
//using UXC.Plugins.SessionsAPI.Recording.Configuration;
//using UXC.Sessions;

//namespace UXC.Plugins.SessionsAPI.Recording
//{
//    class InMemorySessionDeviceRecorder : IDeviceObserver
//    {
//        private readonly List<DeviceType> _devices;
//        private readonly InMemoryRecordingDataAccessBuffer _buffer;
//        private readonly BufferSessionRecorderConfiguration _configuration;

//        public InMemorySessionDeviceRecorder(SessionRecording recording, InMemoryRecordingDataAccessBuffer buffer, BufferSessionRecorderConfiguration configuration)
//        {
//            _devices = recording.SelectedDevices.ToList();
//            _buffer = buffer;
//            _configuration = configuration;
//        }

//        public IDisposable Connect(IObservableDevice device)
//        {
//            var writer = new InMemoryDataBufferWriter(device.DeviceType, _buffer, device.DataType);

//            var bufferedWriter = new BufferedDataWriter(writer, TimeSpan.FromMilliseconds(_configuration.BufferSize));

//            return new CompositeDisposable
//            (
//                device.Data.Do(d => bufferedWriter.Write(d))
//                           .Finally(bufferedWriter.Close)
//                           .Subscribe(),
//                bufferedWriter, 
//                writer
//            );

//            //return Observable.Using
//            //(
//            //    () => new InMemoryDataBufferWriter(device.DeviceType, _buffer, device.RecordingDataType),
//            //    writer => Observable.Using
//            //    (
//            //        () => new BufferedDataWriter(writer, TimeSpan.FromMilliseconds(_configuration.BufferSize)),
//            //        bufferedWriter => device.Data.Do(d => bufferedWriter.Write(d))
//            //                                     .Finally(bufferedWriter.Close)
//            //    )
//            //).Subscribe();
//        }

//        public bool IsDeviceSupported(DeviceType type)
//        {
//            return _devices.Contains(type);
//        }
//    }
//}
