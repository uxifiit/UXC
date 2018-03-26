using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using UXC.Core.Data;
using UXC.Plugins.SessionsAPI.Services;

namespace UXC.Plugins.SessionsAPI.Controllers
{
    [RoutePrefix(ApiRoutes.Recording.DataBuffer.PREFIX)]
    [LocalDateTimeJsonConverterAttribute]
    public class SessionRecordingDataBufferController : ApiController
    {
        private readonly SessionRecordingDataBufferService _service;

        public SessionRecordingDataBufferController(SessionRecordingDataBufferService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route(ApiRoutes.Recording.DataBuffer.ACTION_DATA)]
        [ResponseType(typeof(DeviceData))]
        public IHttpActionResult GetRecordingData(string recording, string device = "ET", [FromUri] DateTime? from = null)
        {
            DeviceData data;

            if (String.IsNullOrWhiteSpace(recording) == false
                && (recording.Equals(ApiRoutes.Recording.PARAM_RECENT_RECORDING, StringComparison.InvariantCultureIgnoreCase)
                    && _service.TryGetData(device, out data, from))
                || _service.TryGetData(recording, device, out data, from))
            {
                return Ok(data);
            }
            return NotFound();
        }
    }
}
