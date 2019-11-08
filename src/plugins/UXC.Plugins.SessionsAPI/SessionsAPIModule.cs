/**
 * UXC.Plugins.SessionsAPI
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ninject.Modules;
using UXC.Core;
using UXC.Core.Devices;
using UXC.Plugins.SessionsAPI.Controllers;
using UXC.Plugins.SessionsAPI.Entities;
using UXC.Plugins.SessionsAPI.Hubs;
using UXC.Plugins.SessionsAPI.Recording;
using UXC.Plugins.SessionsAPI.Services;
using UXC.Sessions;
using UXC.Sessions.Recording;
using UXC.Sessions.Serialization.Converters.Json;
using UXC.Sessions.Timeline;

namespace UXC.Plugins.SessionsAPI
{
    public class SessionsAPIModule : NinjectModule
    {
        public override void VerifyRequiredModulesAreLoaded()
        {
            if (Kernel.HasModule(typeof(SessionsModule).FullName) == false)
            {
                throw new InvalidProgramException($"Required {nameof(SessionsModule)} has not been loaded.");
            }
            base.VerifyRequiredModulesAreLoaded();
        }

        private static IMapper CreateEntitiesMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SessionDeviceDefinition, SessionDeviceDefinitionInfo>()
                   .ForMember(i => i.Device,
                              e => e.MapFrom(definition => definition.Device.Code));
                   //.ForMember(i => i.Configuration)
                                  //e => e.ResolveUsing((SessionDeviceDefinition definition) => definition.Configuration
                                                                          //.Select(d => new SessionDeviceConfigurationSettingInfo() { Setting = d.Key, Value = Convert.ToString(d.Value) })));

                cfg.CreateMap<SessionDeviceDefinitionInfo, SessionDeviceDefinition>()
                   .ConstructUsing((SessionDeviceDefinitionInfo info) =>
                   {
                       DeviceType device;
                       if (DeviceType.TryResolveExistingType(info.Device, out device))
                       {
                           return new SessionDeviceDefinition
                           (
                               device, 
                               configuration: info.Configuration?.ToDictionary(c => c.Key, c => c.Value) ?? new Dictionary<string, object>()
                           );
                       }
                       return null;
                   });

                cfg.CreateMap<SessionRecorderDefinition, SessionRecorderDefinitionInfo>();

                cfg.CreateMap<SessionRecorderDefinitionInfo, SessionRecorderDefinition>()
                   .ConstructUsing((SessionRecorderDefinitionInfo info) =>
                   {
                       return new SessionRecorderDefinition(info.Name, info.Configuration);  
                   });

                cfg.CreateMap<SessionDefinition, SessionDefinitionInfo>();

                cfg.CreateMap<SessionDefinitionCreate, SessionDefinition>()
                   .ConstructUsing((SessionDefinitionCreate _) => SessionDefinition.Create());

                cfg.CreateMap<SessionRecording, SessionRecordingInfo>();

                cfg.CreateMap<SessionStepExecution, SessionStepExecutionInfo>()
                   .ForMember(i => i.ActionType,
                              e => e.MapFrom(execution => execution.Step.Action.ActionType))
                   .ForMember(i => i.Tag,
                              e => e.MapFrom(execution => execution.Step.Action.Tag));

            });

            return config.CreateMapper();
        }

        public override void Load()
        {
            Bind<IMapper>().ToMethod(_ => CreateEntitiesMapper())
                           .WhenInjectedExactlyInto(typeof(SessionDefinitionsService), typeof(SessionsControlService))
                           .InSingletonScope();

            Bind<Newtonsoft.Json.JsonConverter>().To<SingleOrArrayConverter<SessionDeviceDefinitionInfo>>();
            Bind<Newtonsoft.Json.JsonConverter>().To<SingleOrArrayConverter<SessionRecorderDefinitionInfo>>();

            Bind<ISessionDefinitionsSource, ExternalSessionDefinitions>().To<ExternalSessionDefinitions>().InSingletonScope();

            Bind<SessionRecordingResults>().ToSelf()
                                           .WhenInjectedExactlyInto(typeof(SessionRecordingResultsControlService), typeof(SessionRecordingResultsService))
                                           .InSingletonScope();

            Bind<SessionDefinitionsService>().ToSelf()
                                            .WhenInjectedExactlyInto(typeof(SessionDefinitionController), typeof(SessionDefinitionHub))
                                            .InSingletonScope();

            Bind<SessionsControlService>().ToSelf()
                                           .WhenInjectedExactlyInto(typeof(SessionRecordingController), typeof(SessionRecordingTimelineController), typeof(SessionRecordingHub))
                                           .InSingletonScope();

            Bind<SessionRecordingResultsService>().ToSelf()
                                      .WhenInjectedExactlyInto(typeof(SessionRecordingDataController)) // TODO SessionDataHub
                                      .InSingletonScope();

            Bind<SessionRecordingSettingsService>().ToSelf()
                                                   .WhenInjectedExactlyInto(typeof(SessionRecordingSettingsController)/*, typeof(SessionRecordingSettingsHub)*/)
                                                   .InSingletonScope();

            Bind<IControlService>().To<SessionRecordingResultsControlService>().InSingletonScope();

            Bind<SessionDefinitionController>().ToSelf();
            Bind<SessionRecordingController>().ToSelf();
            Bind<SessionRecordingTimelineController>().ToSelf();
            Bind<SessionRecordingDataController>().ToSelf();
            Bind<SessionRecordingSettingsController>().ToSelf();

            Bind<SessionDefinitionHub>().ToSelf();
            Bind<SessionRecordingHub>().ToSelf();
            //Bind<SessionRecordingSettingsHub>().ToSelf();

            // TODO Serialization DELETE
            // BindInMemorySessionRecording();
        }


        // TODO Serialization DELETE
        //private void BindInMemorySessionRecording()
        //{
        //    Bind<InMemoryRecordingDataSource>().ToSelf().InSingletonScope();

        //    Bind<ISessionRecorderFactory>().To<InMemorySessionRecorderFactory>().InSingletonScope();

        //    Bind<SessionRecordingDataBufferService>().ToSelf()
        //                              .WhenInjectedExactlyInto(typeof(SessionRecordingDataBufferController), typeof(SessionRecordingDataBufferHub))
        //                              .InSingletonScope();

        //    Bind<SessionRecordingDataBufferController>().ToSelf();

        //    Bind<SessionRecordingDataBufferHub>().ToSelf();
        //}
    }
}
