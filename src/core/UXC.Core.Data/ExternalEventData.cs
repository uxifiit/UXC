using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Data
{
    public class ExternalEventData : DeviceData
    {
        public ExternalEventData(string sourceName, string eventName, string eventData, DateTime timestamp, DateTime? validTill)
            : base(timestamp)
        {
            Source = sourceName;
            EventName = eventName;
            EventData = eventData;

            ValidTill = validTill;
        }

        public string Source { get; }

        public string EventName { get; }

        public string EventData { get; }

        public DateTime? ValidTill { get; }
    }
}
