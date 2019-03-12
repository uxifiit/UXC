/**
 * UXC.Devices.EyeTracker.Driver.Simulator
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

namespace UXC.Devices.EyeTracker.Driver.Simulator.Extensions
{
    internal static class BytesConverter
    {
        public static byte[] Convert<T>(T[] values)
            where T : struct
        {
            var result = new byte[values.Length * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
            Buffer.BlockCopy(values, 0, result, 0, result.Length);
            return result;
        }

        public static T[] ConvertBack<T>(byte[] bytes)
            where T : struct
        {
            var result = new T[bytes.Length / System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
            Buffer.BlockCopy(bytes, 0, result, 0, bytes.Length);
            return result;
        }
    }
}
