/**
 * UXC.Core.Data.Serialization
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, COPYING.LESSER, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using UXI.Common;

namespace UXC.Core.Data.Serialization
{
    public class BufferedDataWriter : DisposableBase, IDataWriter
    {
        private readonly IDataWriter _writer;
        private readonly Subject<object> _subject;

        private IDisposable _subscription;

        public BufferedDataWriter(IDataWriter writer, int bufferSize)
        {
            _writer = writer;

            _subject = new Subject<object>();

            _subscription = _subject.Buffer(bufferSize)
                                    .Subscribe(b => _writer.WriteRange(b));
        }


        public BufferedDataWriter(IDataWriter writer, TimeSpan timeSpan)
        {
            _writer = writer;

            _subject = new Subject<object>();

            _subscription = _subject.Buffer(timeSpan)
                                    .Subscribe(b => _writer.WriteRange(b));
        }


        public bool CanWrite(Type objectType)
        {
            return _writer.CanWrite(objectType);
        }


        public void Close()
        {
            _subject.OnCompleted();
            _writer.Close();
        }

        public void Write(object data)
        {
            _subject.OnNext(data);
        }


        public void WriteRange(IEnumerable<object> data)
        {
            foreach (var item in data)
            {
                _subject.OnNext(item);   
            }
        }


        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    Close();

                    _subject.Dispose();
                    _subscription?.Dispose();
                    _subscription = null;
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
