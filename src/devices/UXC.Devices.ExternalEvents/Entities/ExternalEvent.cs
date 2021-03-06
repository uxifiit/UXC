/**
 * UXC.Devices.ExternalEvents
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
using Newtonsoft.Json.Linq;

namespace UXC.Devices.ExternalEvents.Entities
{
    [DataContract(Name = "Event")]
    public class ExternalEvent
    {
        [DataMember(Name = "System")]
        public string System { get; set; }           

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Data")]
        public JRaw Data { get; set; }

        [DataMember(Name = "Timestamp", IsRequired = false)]
        public long? Timestamp { get; set; }

        [DataMember(Name = "ValidTill", IsRequired = false)]
        public long? ValidTill { get; set; }
    }
}
