using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Plugins.GazeAccess
{
    static class ApiRoutes
    {
        public const string PREFIX = "api/gaze";

        public static class GazeData
        {
            public const string PREFIX = ApiRoutes.PREFIX;

            // GET /api/gaze/
            public const string ACTION_RECENT = "";
        }
    }
}
