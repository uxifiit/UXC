using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using UXC.Models.Contexts;
using UXC.Plugins.DefaultAPI.Entities;
using UXC.Plugins.DefaultAPI.Services;

namespace UXC.Plugins.DefaultAPI.Controllers
{
    [RoutePrefix(ApiRoutes.Devices.PREFIX)]
    public class DeviceController : ApiController
    {
        private readonly DeviceService _service;

        public DeviceController(DeviceService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route(ApiRoutes.Devices.ACTION_LIST)]
        [ResponseType(typeof(List<DeviceStatusInfo>))]
        public IHttpActionResult GetList()
        {
            return Ok(_service.GetList());
        }


        [HttpGet]
        [Route(ApiRoutes.Devices.ACTION_DETAILS)]
        [ResponseType(typeof(DeviceStatusInfo))]
        public IHttpActionResult GetDetails(string deviceTypeCode)
        {
            try
            {
                return Ok(_service.GetDetails(deviceTypeCode));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
    }
}
