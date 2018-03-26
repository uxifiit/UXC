using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UxLabClass.Adapters.Entities.Data.Emotions
{
    /// <summary>
    /// Kody pre emocie tvare. 
    /// </summary>
    /// <remarks>
    /// Inspired by the code example here:
    /// http://stackoverflow.com/questions/424366/c-sharp-string-enums?rq=1
    /// </remarks>
    [Serializable]
    public sealed class EmotionType : IComparable<EmotionType>
    {
        private static readonly Dictionary<string, EmotionType> instance = new Dictionary<string, EmotionType>();

        private readonly string code;
        public string Code { get { return code; } }
        private readonly int value;

        public static readonly EmotionType ANGER = new EmotionType(0, "ANGER");
        public static readonly EmotionType CONTEMPT = new EmotionType(1, "CONTEMPT");
        public static readonly EmotionType DISGUST = new EmotionType(2, "DISGUST");
        public static readonly EmotionType FEAR = new EmotionType(3, "FEAR");
        public static readonly EmotionType JOY = new EmotionType(4, "JOY");
        public static readonly EmotionType SADNESS = new EmotionType(5, "SAD");
        public static readonly EmotionType SURPRISE = new EmotionType(6, "SURPRISE");
        public static readonly EmotionType NONE = new EmotionType(7, "NONE");


        private EmotionType(int value, string code)
        {
            this.code = code;
            this.value = value;
            instance[code] = this;
        }

        public static EmotionType Resolve(string code)
        {
            return instance[code];
        }

        public override string ToString()
        {
            return code;
        }


        public static explicit operator EmotionType(string str)
        {
            EmotionType result;
            if (instance.TryGetValue(str, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        #region IComparable<Emotion> Members

        public int CompareTo(EmotionType other)
        {
            return value.CompareTo(other.value);
        }

        #endregion


    }

  
}
