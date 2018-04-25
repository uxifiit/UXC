using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace UXC.Server
{
    class ProtectedData : IProtectedData
    {
        public string Protect(string data, string purpose)
        {
            return data;
        }

        public string Unprotect(string protectedValue, string purpose)
        {
            return protectedValue;
        }
    }
}
