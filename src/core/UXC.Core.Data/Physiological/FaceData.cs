using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UxLabClass.Adapters.Entities.Data.Emotions
{

    [Serializable]
    public class FaceData : IComparable<FaceData>, IEquatable<FaceData>
    {

        [DataMember(Name = "FaceIndex")]
        [JsonProperty("FaceIndex")]
        public int FaceIndex { get; set; }

        [DataMember(Name = "Emotion")]
        [JsonProperty("Emotion")]
        public string EmotionCode { get { return Emotion.Code; } set { Emotion = Emotions.EmotionType.Resolve(value); } }

        [IgnoreDataMember]
        [JsonIgnore]
        public EmotionType Emotion
        {
            get;
            set;
        }

        [DataMember(Name = "Sentiment")]
        [JsonProperty("Sentiment")]
        public string SentimentCode
        {
            get { return Sentiment.Code; }
            set { Sentiment = SentimentType.Resolve(value); }
        }

        [IgnoreDataMember]
        [JsonIgnore]
        public SentimentType Sentiment { get; set; }

        [DataMember(Name = "FacePosition")]
        [JsonProperty("FacePosition")]
        public PointTwoD FacePosition { get; set; }

        public FaceData()
        {
            FaceIndex = 0;
            Emotion = EmotionType.NONE;
            Sentiment = SentimentType.NONE;
        }

        public FaceData(int index)
            : this()
        {
            FaceIndex = index;
        }


        public bool Equals(FaceData f2)
        {
            if (Object.ReferenceEquals(f2, null))
                return false;
            if (Object.ReferenceEquals(this, f2))
                return true;
            return Emotion == f2.Emotion && Sentiment == f2.Sentiment;
        }

        public override int GetHashCode()
        {
            return Emotion.GetHashCode() * 17 + Sentiment.GetHashCode();
        }

        public int CompareTo(FaceData face)
        {
            if (face.Emotion.CompareTo(Emotion) < 0)
            {
                return 1;
            }
            else if (face.Emotion == Emotion)
            {
                if (face.Sentiment.CompareTo(Sentiment) < 0)
                {
                    return 1;
                }

                if (face.Sentiment.CompareTo(Sentiment) == 0)
                {
                    return 0;
                }
            }

            return -1;
        }
    }
}
