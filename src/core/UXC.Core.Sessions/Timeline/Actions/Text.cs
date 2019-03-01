using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UXC.Sessions.Timeline.Actions
{
    public class Text : IEnumerable<string>
    {
        public List<string> Lines { get; set; }

        public IEnumerator<string> GetEnumerator()
        {
            return Lines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return Lines != null && Lines.Any()
                 ? String.Join(Environment.NewLine, Lines)
                 : String.Empty;
        }
    }   
}
