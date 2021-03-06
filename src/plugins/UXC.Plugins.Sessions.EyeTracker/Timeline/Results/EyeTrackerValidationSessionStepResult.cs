/**
 * UXC.Plugins.Sessions.EyeTracker
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
using UXC.Core.Data;
using UXI.Common.Extensions;

namespace UXC.Sessions.Timeline.Results
{
    public class EyeTrackerValidationSessionStepResult : SessionStepResult
    {
        public EyeTrackerValidationSessionStepResult(SessionStepResultType result, IEnumerable<PointDisplayTime> displayedPoints)
            : base(result)
        {
            Points = displayedPoints.ThrowIfNull(nameof(displayedPoints)).ToList();
        }

        public List<PointDisplayTime> Points { get; }
    }


    public class PointDisplayTime
    {
        public PointDisplayTime(Point2 point, DateTime startTime, DateTime endTime)
        {
            Point = point;
            StartTime = startTime;
            EndTime = endTime;
        }

        public Point2 Point { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
    }
}
