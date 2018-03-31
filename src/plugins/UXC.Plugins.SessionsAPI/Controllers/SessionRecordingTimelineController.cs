using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using UXC.Plugins.SessionsAPI.Services;
using UXC.Sessions.Timeline;

namespace UXC.Plugins.SessionsAPI.Controllers
{
    [RoutePrefix(ApiRoutes.Recording.Timeline.PREFIX)]
    public class SessionRecordingTimelineController : ApiController
    {
        private readonly SessionsControlService _service;

        public SessionRecordingTimelineController(SessionsControlService service)
        {
            _service = service;
        }


        [HttpPost]
        [Route(ApiRoutes.Recording.Timeline.ACTION_CONTINUE)]
        public IHttpActionResult Continue()
        {
            _service.Continue();
            return Ok();
        }



        [HttpPost]
        [Route(ApiRoutes.Recording.Timeline.ACTION_STEP)]
        public IHttpActionResult ContinueStep([FromBody] SessionStep step)
        {
            _service.Continue(step);
            return Ok();
        }
    }
}
