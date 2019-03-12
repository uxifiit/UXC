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
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Configuration;

namespace UXC.Sessions.Recording
{               
    public interface ISessionRecorder : IConfigurable, IDisposable
    {
        void Record();
        //void Close();
        event EventHandler<ISessionRecordingResult> Closed;
    }
}
