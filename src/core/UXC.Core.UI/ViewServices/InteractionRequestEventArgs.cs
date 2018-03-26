using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.ViewServices
{
    public class InteractionRequestEventArgs
    {
        internal InteractionRequestEventArgs(object source, InteractionRequest request)
        {
            Source = source;
            Request = request;
        }


        public object Source { get; }

        public object Request { get; }

        public bool IsAccepted { get; set; }
    }


    public class InteractionRequest
    {
        public static readonly InteractionRequest Empty = new InteractionRequest();

        protected InteractionRequest() { }
    }
}
