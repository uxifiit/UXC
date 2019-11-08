///**
// * UXC.Core.Data.Serialization
// * Copyright (c) 2018 The UXC Authors
// * 
// * Licensed under GNU Lesser General Public License 3.0 only.
// * Some rights reserved. See COPYING, COPYING.LESSER, AUTHORS.
// *
// * SPDX-License-Identifier: LGPL-3.0-only
// */
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace UXC.Core.Data.Serialization
//{
//    public static class IDataReaderEx
//    {
//        public static bool TryRead<T>(this IDataReader reader, out T data)
//        {
//            return reader.TryRead(out data);
//        }

//        public static IEnumerable<object> ReadAll(this IDataReader reader, Type objectType)
//        {
//            if (reader.CanRead(objectType))
//            {
//                object data;
//                while (reader.TryRead(out data))
//                {
//                    yield return data;
//                }
//            }
//        }

//        public static IEnumerable<T> ReadAll<T>(this IDataReader reader)
//        {
//            return ReadAll(reader, typeof(T)).OfType<T>();
//        }
//    }
//}
