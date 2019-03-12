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
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Plugins.SessionsAPI.Entities
{
    [DataContract(Name = "Session")]
    public class SessionDefinitionCreate
    {
        [DataMember]
        public string Project { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember(IsRequired = false)]
        public string Source { get; set; }

        [DataMember]
        public string SerializationFormat { get; set; }

        [DataMember]
        public bool StrictStart { get; set; }

        [DataMember(IsRequired = false)]
        public bool Save { get; set; } = true;

        [DataMember]
        public WelcomeActionSettings Welcome { get; set; }

        [DataMember]
        public List<SessionStep> SessionSteps { get; set; }

        [DataMember]
        public List<SessionStep> PreSessionSteps { get; set; }

        [DataMember]
        public List<SessionStep> PostSessionSteps { get; set; }


        [DataMember]
        public List<SessionRecorderDefinitionInfo> Recorders { get; set; } = new List<SessionRecorderDefinitionInfo>();

        [DataMember]
        public List<SessionDeviceDefinitionInfo> Devices { get; set; } = new List<SessionDeviceDefinitionInfo>();
    }
}
