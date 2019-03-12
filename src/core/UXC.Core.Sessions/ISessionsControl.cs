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
using System.Threading;
using System.Threading.Tasks;

namespace UXC.Sessions
{
    public interface ISessionsControl
    {
        SessionRecording CurrentRecording { get; }
        IObservable<SessionRecording> Recordings { get; }

        IObservable<ISessionRecordingResult> CompletedRecordings { get; }

        event EventHandler<SessionRecording> RecordingChanged;

        void Close();

        void Clear();

        SessionRecording Record(SessionDefinition definition);
    }
}
