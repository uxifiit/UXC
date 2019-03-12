/**
 * UXC.Core.Sessions.UI
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System.Threading.Tasks;

namespace UXC.Sessions.ViewModels.Timeline.Preparation
{
    public interface ITimelineStepPreparation
    {
        Task PrepareAsync(SessionRecording recording);

        void Reset();
    }
}