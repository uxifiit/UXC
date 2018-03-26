using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UxLabClass.Adapters.Entities.Data.Emotions
{
    /// <summary>
    /// Kody pre sentiment emocie tvare. 
    /// </summary>
    [Serializable]
    public sealed class SentimentType : IComparable<SentimentType>
    {
        private static readonly Dictionary<string, SentimentType> instance = new Dictionary<string, SentimentType>();

        private readonly string code;
        public string Code { get { return code; } }

        private readonly int value;

        public static readonly SentimentType NEGATIVE = new SentimentType(0, "NEGATIVE");
        public static readonly SentimentType POSITIVE = new SentimentType(1, "POSITIVE");
        public static readonly SentimentType NEUTRAL = new SentimentType(2, "NEUTRAL");
        public static readonly SentimentType NONE = new SentimentType(3, "NONE");


        private SentimentType(int value, string code)
        {
            this.code = code;
            this.value = value;
            instance[code] = this;
        }

        public static SentimentType Resolve(string code)
        {
            return instance[code];
        }

        public override string ToString()
        {
            return code;
        }

        public static explicit operator SentimentType(string str)
        {
            SentimentType result;
            if (instance.TryGetValue(str, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        #region IComparable<Sentiment> Members

        public int CompareTo(SentimentType other)
        {
            return value.CompareTo(other.value);
        }

        #endregion
    }
}
