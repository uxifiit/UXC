/**
 * UXC.Devices.KeyboardMouse
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

namespace UXC.Devices.KeyboardMouse
{
    class TicksToTimestampConverter
    {
        private int _recentTicks;
        private DateTime _recentTimestamp;

        public TicksToTimestampConverter()
        {
            Reset();
        }


        public DateTime Recent => _recentTimestamp;


        public void Reset()
        {
            _recentTicks = -1;
            _recentTimestamp = DateTime.Now;
        }


        public DateTime Convert(int ticks)
        {
            DateTime timestamp;
            if (_recentTicks == ticks)
            {
                timestamp = _recentTimestamp;
            }
            else
            {
                _recentTicks = ticks;
                timestamp = _recentTimestamp = DateTime.Now;
            }

            return timestamp;
        }
    }
}
