using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Data
{
    public class MouseEventData : DeviceData
    {
        public MouseEventData(MouseEventType type, int x, int y, MouseButton button, int delta, DateTime timestamp) 
            : base(timestamp)
        {
            EventType = type;
            X = x;
            Y = y;
            Button = button;
            Delta = delta;
        }

        public MouseEventType EventType { get; }

        /// <summary>
        /// Gets or sets which mouse button was pressed.
        /// </summary>
        public MouseButton Button { get; }

        public int Delta { get; }

        public int X { get; }

        public int Y { get; }
    }
}
