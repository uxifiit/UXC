using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Plugins.DefaultAPI
{
    static class ApiRoutes
    {
        public const string PREFIX = "api";


        public static class Devices
        {
            public const string PREFIX = ApiRoutes.PREFIX + "/device";

            public const string PARAM_DEVICE_TYPE_CODE = @"{deviceTypeCode:regex([a-zA-Z]+)}";

            // api/device/
            public const string ACTION_LIST = "";

            // api/device/ET/
            public const string ACTION_DETAILS = PARAM_DEVICE_TYPE_CODE + "/";
        }
    }
}
