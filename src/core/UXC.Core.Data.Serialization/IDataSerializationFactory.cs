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
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace UXC.Core.Data.Serialization
//{
//    public interface IDataSerializationFactory
//    {
//        string FileExtension { get; }

//        string FormatName { get; }

//        string MimeType { get; }

//        IDataWriter CreateWriterForType(TextWriter writer, Type dataType);

//        IDataReader CreateReaderForType(TextReader reader, Type dataType);
//    }
//}
