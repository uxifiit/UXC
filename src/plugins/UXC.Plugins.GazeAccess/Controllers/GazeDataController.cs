using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using UXC.Core.Data;
using UXC.Plugins.GazeAccess.Observers;

namespace UXC.Plugins.GazeAccess.Controllers
{
    [RoutePrefix(ApiRoutes.PREFIX)]
    public class GazeDataController : ApiController
    {
        private readonly EyeTrackerObserver _eyeTracker;

        public GazeDataController(EyeTrackerObserver eyeTracker)
        {
            _eyeTracker = eyeTracker;
        }

        // GET api/gaze/data/
        [HttpGet]
        [Route(ApiRoutes.GazeData.ACTION_RECENT)]
        [ResponseType(typeof(GazeData))]
        public IHttpActionResult GetRecent()
        {
            return Ok(_eyeTracker.RecentData);
        }
    }
}
