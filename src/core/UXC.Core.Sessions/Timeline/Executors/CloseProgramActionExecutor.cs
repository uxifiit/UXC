﻿/**
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
using UXC.Sessions.Helpers;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.Timeline.Executors
{
    public class CloseProgramActionExecutor : SessionStepActionExecutor<CloseProgramActionSettings>
    {
        private readonly IProcessService _service;

        public CloseProgramActionExecutor(IProcessService service)
        {
            _service = service;
        }


        protected override void Execute(SessionRecording recording, CloseProgramActionSettings settings)
        {
            string tag = settings.Tag;

            if (String.IsNullOrWhiteSpace(tag))
            {
                _service.CloseAll(settings.ForceClose, settings.ForceCloseTimeout);
            }
            else
            {
                _service.Close(tag, settings.ForceClose, settings.ForceCloseTimeout);
            }

            // possible result options: process closed, process was not running, process killed
            Complete();
        }
    }
}
