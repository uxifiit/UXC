using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using UXC.Core.Data;
using UXC.Devices.ExternalEvents.Entities;
using UXC.Devices.ExternalEvents.Services;

namespace UXC.Devices.ExternalEvents.Controllers
{
    [RoutePrefix(ApiRoutes.ExternalEvents.PREFIX)]
    public class EventController : ApiController
    {
        private readonly EventService _service;

        public EventController(EventService service)
        {
            _service = service;
        }


        [HttpPost]
        [Route(ApiRoutes.ExternalEvents.ACTION_SEND)]
        public IHttpActionResult SendEvent([FromBody] ExternalEvent @event)
        {
            try
            {
                _service.Record(@event);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route(ApiRoutes.ExternalEvents.ACTION_SEND_MANY)]
        public IHttpActionResult SendMany([FromBody] List<ExternalEvent> events)
        {
            try
            {
                _service.RecordMany(events);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
