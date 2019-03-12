/**
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
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Sessions.Extensions
{
    public static class IObservableEx
    {
        public static IObservable<TEventArgs> FirstOrDefaultEventAsync<TEventArgs>(Action<EventHandler<TEventArgs>> addHandler, Action<EventHandler<TEventArgs>> removeHandler)
        {
            return Observable.FromEventPattern<TEventArgs>(addHandler, removeHandler).FirstOrDefaultAsync().Select(e => e == null ? default(TEventArgs) : e.EventArgs);
        }
    }
}
