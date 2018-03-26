using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Devices.ExternalEvents
{
    static class ApiRoutes
    {
        public const string PREFIX = "api/event";

        public static class ExternalEvents
        {
            public const string PREFIX = ApiRoutes.PREFIX;

            public const string ACTION_SEND = "";

            public const string ACTION_SEND_MANY = "many";
        }
    }
}
