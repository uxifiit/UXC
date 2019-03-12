/**
 * UXC.Devices.ExternalEvents
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
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;
using UXC.Devices.ExternalEvents.Entities;

namespace UXC.Devices.ExternalEvents
{
    public interface IEventsRecorder
    {
        bool IsOpen { get; }

        IObservable<ExternalEventData> Events { get; }

        void Open();

        void Close();

        void Record(ExternalEvent ev);
    }
}
