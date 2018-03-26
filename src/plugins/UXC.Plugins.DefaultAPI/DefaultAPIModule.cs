using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ninject.Modules;
using UXC.Core.Devices;
using UXC.Core.Models;
using UXC.Plugins.DefaultAPI.Controllers;
using UXC.Plugins.DefaultAPI.Entities;
using UXC.Plugins.DefaultAPI.Hubs;
using UXC.Plugins.DefaultAPI.Services;

namespace UXC.Plugins.DefaultAPI
{
    public class DefaultAPIModule : NinjectModule
    {
        private static IMapper CreateEntitiesMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DeviceStatus, DeviceStatusInfo>()
                   .ForMember(i => i.Device,
                              e => e.MapFrom(status => status.DeviceType.Code));
            });

            return config.CreateMapper();
        }


        public override void Load()
        {
            Bind<IMapper>().ToMethod(_ => CreateEntitiesMapper())
                           .WhenInjectedExactlyInto(typeof(DeviceService))
                           .InSingletonScope();

            Bind<DeviceService>().ToSelf()
                                 .WhenInjectedExactlyInto(typeof(DeviceController), typeof(DeviceHub))
                                 .InSingletonScope();

            Bind<DeviceController>().ToSelf();
            Bind<DeviceHub>().ToSelf();
        }
    }
}
