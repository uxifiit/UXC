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
            current = initialValue;
            _stepFunc = stepFunc;
        }


        private T current;
        public T Current => current;


        object IEnumerator.Current
        {
            get
            {
                return current;
            }
        }


        public bool MoveNext()
        {
            current = _stepFunc.Invoke(current);
            return true;
        }


        public void Reset()
        {
            current = _initialValue;
        }


        public void Dispose() { }
    }
}
