/**
 * UXC.Plugins.SessionsAPI
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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Plugins.SessionsAPI.Entities
{
    [DataContract(Name = "SessionStep")]
    public class SessionStepExecutionInfo
    {
        //[DataMember]
        //public int Index { get; set; }

        [DataMember]
        public string ActionType { get; set; }
        
        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public DateTime StartedAt { get; set; }
    }
}
