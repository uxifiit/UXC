/**
 * UXC.Devices.EyeTracker.Calibration
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;

namespace UXC.Devices.EyeTracker.Calibration
{
    public class CalibrationPlan : IEnumerable<Point2>
    {
        private readonly List<Point2> _points;

        public CalibrationPlan(IEnumerable<Point2> points)
        {
            Length = points.Count();
            _points = points.ToList();
        }

        public int Length { get; }

        public IEnumerator<Point2> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
