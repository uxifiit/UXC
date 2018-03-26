using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Plugins.SessionsAPI.Entities
{
    [DataContract(Name = "Recording")]
    public class SessionRecordingInfo
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember(IsRequired = true)]
        public string State { get; set; }

        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public DateTime? StartedAt { get; set; } = null;

        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public bool IsFinished { get; set; } = false;

        [DataMember]
        public string CurrentStep { get; set; }

        [DataMember(IsRequired = true)]
        public SessionDefinitionInfo Definition { get; set; }
    }
}
