using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using UXC.Core.Devices;
using UXC.Plugins.SessionsAPI.Entities;
using UXC.Plugins.SessionsAPI.Services;
using UXC.Sessions;
using UXC.Sessions.Timeline;

namespace UXC.Plugins.SessionsAPI.Controllers
{
    [RoutePrefix(ApiRoutes.Recording.PREFIX)]
    public class SessionRecordingController : ApiController
    {
        private readonly SessionsControlService _service;

        public SessionRecordingController(SessionsControlService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route(ApiRoutes.Recording.ACTION_INFO)]
        [ResponseType(typeof(SessionRecordingInfo))]
        public IHttpActionResult Info()
        {
            return Ok(_service.GetCurrentRecording());
        }


        [HttpGet]
        [Route(ApiRoutes.Recording.ACTION_STATUS)]
        [ResponseType(typeof(SessionState))]
        public IHttpActionResult GetStatus()
        {
            return Ok(_service.GetCurrentRecording()?.State ?? SessionState.None.ToString());
        }


        [HttpGet]
        [Route(ApiRoutes.Recording.ACTION_ID)]
        [ResponseType(typeof(string))]
        public IHttpActionResult GetId()
        {
            return Ok(_service.GetCurrentRecording()?.Id);
        }


        [HttpPost]
        [Route(ApiRoutes.Recording.ACTION_OPEN_EXISTING)]
        [ResponseType(typeof(SessionRecordingInfo))]
        public IHttpActionResult OpenExistingSession(int definitionId)
        {
            try
            {
                _service.Open(definitionId);

                return Ok(_service.GetCurrentRecording()); 
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }


        [HttpPost]
        [Route(ApiRoutes.Recording.ACTION_OPEN_CREATE)]
        [ResponseType(typeof(SessionRecordingInfo))]
        public IHttpActionResult OpenSession([FromBody] SessionDefinitionCreate session)
        {
            try
            {
                _service.Open(session);

                return Ok(_service.GetCurrentRecording());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route(ApiRoutes.Recording.ACTION_CLEAR)]
        public IHttpActionResult Clear()
        {
            _service.Clear();

            return Ok();
        }
    }
}
