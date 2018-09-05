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
