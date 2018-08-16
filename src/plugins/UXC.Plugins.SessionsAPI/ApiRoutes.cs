using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Plugins.SessionsAPI
{
    static class ApiRoutes
    {
        public const string PREFIX = "api/session";


        public static class Definition
        {
            public const string PREFIX = ApiRoutes.PREFIX + "/definition";

            public const string PARAM_DEFINITION_ID = @"{definitionId:regex([0-9]+)}";

            // GET api/session/definition/
            public const string ACTION_LIST = "";

            // POST api/session/definition/
            public const string ACTION_CREATE = "";

            // GET api/session/definition/1/
            public const string ACTION_DETAILS = PARAM_DEFINITION_ID + "/";
        }


        public static class Recording
        {
            public const string PREFIX = ApiRoutes.PREFIX + "/recording";

            public const string DATETIME_FORMAT = "yyyy'-'MM'-'dd'T'HH'_'mm'_'ss";
            public const string DATETIME_REGEX = @"\d{4}-\d{2}-\d{2}T\d{2}_\d{2}_\d{2}";

            public static string ConvertToRouteString(DateTime dateTime)
            {
                return dateTime.ToString(DATETIME_FORMAT);
            }

            public const string PARAM_RECENT_RECORDING = "recent";

            public const string PARAM_RECORDING_ID = "{recording:regex(" + PARAM_RECENT_RECORDING + "|" + DATETIME_REGEX + ")}";

            // GET api/session/recording/
            public const string ACTION_INFO = "";

            // GET api/session/recording/status
            public const string ACTION_STATUS = "status";

            // GET api/session/recording/id
            public const string ACTION_ID = "id";


            // POST api/session/recording/open/<definitionId>
            public const string ACTION_OPEN_EXISTING = "open/" + Definition.PARAM_DEFINITION_ID;

            // POST api/session/recording/open/
            public const string ACTION_OPEN_CREATE = "open";

            // POST api/session/recording/clear/
            public const string ACTION_CLEAR = "clear";

            //// GET api/session/recording/list/
            //public const string ACTION_LIST = "list";


            public static class Timeline
            {
                public const string PREFIX = Recording.PREFIX + "/timeline";

                // POST api/session/recording/timeline/continue
                public const string ACTION_CONTINUE = "continue";
                
                // POST api/session/recording/timeline/step
                public const string ACTION_STEP = "step";
            }

            public static class Settings
            {
                public const string PREFIX = Recording.PREFIX + "/settings";

                public const string PARAM_SECTION = "{section:regex([a-zA-Z0-9_-]+)}";
                public const string PARAM_KEY = "{key:regex([a-zA-Z0-9_-]+)}";

                public const string ACTION_KEY = PARAM_SECTION + "/" + PARAM_KEY;
            }


            public static class Data
            {
                public const string PREFIX = Recording.PREFIX + "/data";

                // GET api/session/recording/data/recent/gaze
                // GET api/session/recording/data/2017-10-26T15_23_42/gaze
                public const string ACTION_DOWNLOAD_GAZE = PARAM_RECORDING_ID + "/gaze";

                // GET api/session/recording/data/recent/fixation
                // GET api/session/recording/data/2017-10-26T15_23_42/fixation
                public const string ACTION_DOWNLOAD_FIXATIONS = PARAM_RECORDING_ID + "/fixation";
            }


            public static class DataBuffer
            {
                public const string PREFIX = Recording.PREFIX + "/buffer";

                public const string PARAM_DEVICETYPE = "{device:regex([a-zA-Z]+)}";

                // GET api/session/recording/recent/buffer/ET
                // GET api/session/recording/2017-10-26T15_23_42/buffer/ET
                public const string ACTION_DATA = PARAM_RECORDING_ID + "/" + PARAM_DEVICETYPE;
            }
        }
    }
}
