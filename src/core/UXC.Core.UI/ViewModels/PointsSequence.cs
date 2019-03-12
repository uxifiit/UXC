/**
 * UXC.Core.UI
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
using System.Windows;

namespace UXC.Core.ViewModels
{
    public class PointsSequence : IEnumerable<Point>
    {
        private readonly List<Point> _points;

        public PointsSequence(IEnumerable<Point> points)
        {
            _points = points?.ToList() ?? new List<Point>();
            Length = _points.Count();
        }

        public int Length { get; }

        public IEnumerator<Point> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
