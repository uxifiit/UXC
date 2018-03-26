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
    public class SessionDefinitionInfo
    {              
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Project { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string SerializationFormat { get; set; }
       
        [DataMember]
        public bool StrictStart { get; set; }

        [DataMember]
        public List<SessionRecorderDefinitionInfo> Recorders { get; set; } = new List<SessionRecorderDefinitionInfo>();

        [DataMember]
        public List<SessionDeviceDefinitionInfo> Devices { get; set; } = new List<SessionDeviceDefinitionInfo>();

        [DataMember]
        public WelcomeActionSettings Welcome { get; set; }

        [DataMember]
        public List<SessionStep> PreSessionSteps { get; set; } = new List<SessionStep>();

        [DataMember]
        public List<SessionStep> SessionSteps { get; set; } = new List<SessionStep>();

        [DataMember]
        public List<SessionStep> PostSessionSteps { get; set; } = new List<SessionStep>();
    }


    [DataContract(Name = "Recorder")]
    public class SessionRecorderDefinitionInfo
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Dictionary<string, object> Configuration { get; set; } = new Dictionary<string, object>();
    }


    [DataContract(Name = "Device")]
    public class SessionDeviceDefinitionInfo
    {
        [DataMember]
        public string Device { get; set; }

        [DataMember]
        public Dictionary<string, object> Configuration { get; set; } = new Dictionary<string, object>();
    }
}
