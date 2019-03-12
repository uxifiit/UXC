/**
 * UXC.Core.Sessions
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
using UXC.Core.Data.Serialization;
using UXC.Core.Managers;
using UXC.Sessions.Serialization;
using UXI.Common.Extensions;

namespace UXC.Sessions.Recording.Local
{
    public class LocalSessionRecorderFactory : ISessionRecorderFactory
    {
        private readonly ISessionsConfiguration _configuration;
        private readonly IObserversManager _observers;
        private readonly List<IDataSerializationFactory> _writers;

        public LocalSessionRecorderFactory(IObserversManager observers, IEnumerable<IDataSerializationFactory> writers, ISessionsConfiguration configuration)
        {
            _observers = observers;
            _writers = writers.ToList();
            _configuration = configuration;
        }

        public string Target => "Local"; // SessionRecorderTarget.Local;

        public ISessionRecorder Create(SessionRecording recording)
        {
            var definition = recording.Definition;
            IDataSerializationFactory writerFactory = _writers.FirstOrDefault(w => w.FormatName.Equals(definition.SerializationFormat, StringComparison.InvariantCultureIgnoreCase));

            if (writerFactory == null)
            {
                // get any writer if the specified format was not found
                writerFactory = _writers.FirstOrDefault();
            }

            writerFactory.ThrowIfNull(() => new ArgumentOutOfRangeException(nameof(definition.SerializationFormat), "No writer for session recording found."));

            return new LocalSessionRecorder(recording, _observers, writerFactory, _configuration);
        }
    }
}
