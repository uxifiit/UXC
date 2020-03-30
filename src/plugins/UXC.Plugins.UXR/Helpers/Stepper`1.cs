/**
 * UXC.Plugins.UXR
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
using UXI.Common.Extensions;

namespace UXC.Plugins.UXR.Helpers
{
    class Stepper<T> : IEnumerator<T>
    {
        private readonly T _initialValue;
        private readonly Func<T, T> _stepFunc;

        public Stepper(T initialValue, Func<T, T> stepFunc)
        {
            stepFunc.ThrowIfNull(nameof(stepFunc));

            _initialValue = initialValue;
            Current = initialValue;
            _stepFunc = stepFunc;
        }


        public T Current { get; private set; }


        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }


        public bool MoveNext()
        {
            Current = _stepFunc.Invoke(Current);
            return true;
        }


        public void Reset()
        {
            Current = _initialValue;
        }


        public void Dispose() { }
    }
}
