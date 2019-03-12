/**
 * UXC.Core.Sessions.UI
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
using UXC.Core.Modules;

namespace UXC.Sessions.ViewModels.Timeline.Preparation
{
    public class TimelinePreparation
    {
        private readonly HashSet<ITimelineStepPreparation> _preparations = new HashSet<ITimelineStepPreparation>();

        public TimelinePreparation(IEnumerable<ITimelineStepPreparation> preparations, IModulesService modules)
        {
            AddPreparations(preparations);

            modules.Register<ITimelineStepPreparation>(this, AddPreparations);
        }


        private void AddPreparations(IEnumerable<ITimelineStepPreparation> preparations)
        {
            preparations?.ForEach(preparation => _preparations.Add(preparation));
        }

        public async Task PrepareAsync(SessionRecording recording)
        {
            foreach (var preparation in _preparations.ToArray())
            {
                await preparation.PrepareAsync(recording);
            }
        }

        public void Reset()
        {
            foreach (var preparation in _preparations.ToArray())
            {
                preparation.Reset();
            }
        }
    }
}
