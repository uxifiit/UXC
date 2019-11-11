/**
 * UXC.Core.Interfaces
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Common.Events
{
    public class ValueChangedEventArgs<T> : TimestampedEventArgs
    {
        public T PreviousValue { get; private set; }
        public T CurrentValue { get; private set; }

        public ValueChangedEventArgs(T current, T previous, DateTime timestamp)
            : base(timestamp)
        {
            if (Object.Equals(current, previous))
            {
                throw new ArgumentException("Values cannot be the same");
            }
            CurrentValue = current;
            PreviousValue = previous;
        }

        public ValueChangedEventArgs(T current, T previous)
            : this(current, previous, DateTime.Now)
        { }
    }
}
