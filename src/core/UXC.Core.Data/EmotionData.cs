/////////////////////////////////////////////////////////////
//                                                         //
//             (c) Softec, spol. s r. o., 2015             //
//                                                         //
/////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UxLabClass.Adapters.Entities.Data;
using System.Drawing;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UxLabClass.Adapters.Entities.Data.Emotions;

namespace UxLabClass.Adapters.Entities.Data
{
    [Serializable]
    public class EmotionData : RecordingData
    {
        [DataMember(Name = "Faces")]
        [JsonProperty("Faces")]
        public FaceData[] Faces { get; private set; }

        private EmotionData() { }

        public EmotionData(IEnumerable<FaceData> faces, DateTime timestamp)
        {
            Faces = faces.ToArray();
            TimeStamp = timestamp;
        }
    }

}
