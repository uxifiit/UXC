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
        public IHttpActionResult Continue([FromBody] List<SessionStep> steps = null)
        {
            List<SessionStep> insertedSteps = null;

            if (steps != null && steps.Any())
            {
                _service.InsertSteps(steps, out insertedSteps, 0);
            }

            _service.Continue();

            return Ok();
        }


        // TODO add API call to insert steps into a specific timeline


        [HttpPost]
        [Route(ApiRoutes.Recording.Timeline.ACTION_INSERT)]
        public IHttpActionResult Insert([FromUri] int position = 0, [FromBody] List<SessionStep> steps = null)
        {
            List<SessionStep> insertedSteps = null;

            if (steps != null && steps.Any())
            {
                if (_service.InsertSteps(steps, out insertedSteps, position))
                {
                    return Ok(insertedSteps);
                }

                return BadRequest("No recording is running.");
            }

            return BadRequest();
        }
    }
}
