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
//using System.Text;
//using System.Threading.Tasks;
//using UXC.Core.Data;

//namespace UXC.Plugins.SessionsAPI.Recording
//{
//    public class DeviceDataPart : DeviceData
//    {
//        public DeviceDataPart(IEnumerable<DeviceData> data)
//            : base(data?.LastOrDefault()?.Timestamp ?? DateTime.MinValue)
//        {
//            Data = data.ToList();
//        }

//        public IReadOnlyList<DeviceData> Data { get; } 
//    }
//}
