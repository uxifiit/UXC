using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UXC.Models.Contexts;
using UXC.Plugins.DefaultAPI.Entities;

namespace UXC.Plugins.DefaultAPI.Services
{
    public class DeviceService
    {
        private readonly IDevicesContext _devices;
        private readonly IMapper _mapper;

        public DeviceService(IDevicesContext devices, IMapper mapper)
        {
            _devices = devices;
            _mapper = mapper;
        }


        public List<DeviceStatusInfo> GetList()
        {
            return _devices.Devices.Select(d => _mapper.Map<DeviceStatusInfo>(d)).ToList();
        }


        public DeviceStatusInfo GetDetails(string deviceTypeCode)
        {
            var deviceStates = _devices.Devices.Where(d => d.DeviceType.Code.Equals(deviceTypeCode, StringComparison.InvariantCultureIgnoreCase));

            if (deviceStates.Any())
            {
                return _mapper.Map<DeviceStatusInfo>(deviceStates.First());
            }

            throw new ArgumentOutOfRangeException(nameof(deviceTypeCode));
        }
    }
}
