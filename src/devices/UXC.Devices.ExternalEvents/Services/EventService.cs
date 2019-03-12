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
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.ExternalEvents.Entities;
using UXI.Common.Extensions;

namespace UXC.Devices.ExternalEvents.Services
{
    public class EventService
    {
        private readonly IEventsRecorder _recorder;
        public EventService(IEventsRecorder recorder)
        {
            _recorder = recorder;
        }


        public void Record(ExternalEvent @event)
        {
            @event.ThrowIfNull(nameof(@event));

            if (@event != null)
            {
                _recorder.Record(@event);
            }
        }

        public void RecordMany(List<ExternalEvent> events)
        {
            events.ThrowIfNull(nameof(events));

            events.Where(e => e != null)
                  .ForEach(d => _recorder.Record(d));
        }
    }
}
