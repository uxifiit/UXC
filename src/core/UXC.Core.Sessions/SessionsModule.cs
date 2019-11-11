/**
 * UXC.Core.Sessions
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
using Ninject.Modules;
using UXC.Core.Data.Serialization;
using UXC.Sessions.Recording;
using UXC.Sessions.Recording.Local;
using UXC.Sessions.Serialization.Converters.Json;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Helpers;
using UXI.Serialization;
using UXI.Serialization.Formats.Json;
using UXC.Core.Data.Serialization.Formats.Csv;
using UXC.Core.Data.Serialization.Formats.Json;
using UXI.Serialization.Formats.Csv;
using UXI.Serialization.Formats.Json.Configurations;
using Ninject;

namespace UXC.Sessions
{
    public class SessionsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISessionsConfiguration>().To<SessionsConfiguration>().InSingletonScope();

            Bind<ISessionRecorderFactory>().To<LocalSessionRecorderFactory>().InTransientScope();
            Bind<SessionRecorderFactoryLocator>().ToSelf().InSingletonScope();

            Bind<SessionDefinitionsSource>().ToSelf().InSingletonScope();
            Bind<ISessionsControl>().To<SessionsControl>().InSingletonScope();

            foreach (var converter in SessionDefinitionJsonConverters.Converters)
            {
                Bind<Newtonsoft.Json.JsonConverter>().ToConstant(converter);
            }

            BindDataSerialization();
            BindLocalSessionDefinitions();

            RegisterTimelineSteps();

            BindProgramsSessionStep();
        }


        private void RegisterTimelineSteps()
        {
            SessionStepActionSettings.Register(typeof(ShowDesktopActionSettings));
            SessionStepActionSettings.Register(typeof(LaunchProgramActionSettings));
            SessionStepActionSettings.Register(typeof(CloseProgramActionSettings));
            SessionStepActionSettings.Register(typeof(QuestionaryActionSettings));
            SessionStepActionSettings.Register(typeof(ChooseAnswerQuestionActionSettings));
            SessionStepActionSettings.Register(typeof(WriteAnswerQuestionActionSettings));
            SessionStepActionSettings.Register(typeof(InstructionsActionSettings));
            SessionStepActionSettings.Register(typeof(ImageActionSettings));
        }


        private void BindLocalSessionDefinitions()
        {
            Bind<ILocalSessionDefinitionsService>().To<LocalSessionDefinitionsService>().InSingletonScope();
        }


        private void BindDataSerialization()
        {
            Bind<ISerializationConfiguration>().ToMethod(s => new JsonConvertersSerializationConfiguration(s.Kernel.GetAll<Newtonsoft.Json.JsonConverter>()));
            Bind<ISerializationConfiguration>().To<UXCDataCsvConvertersSerializationConfiguration>();
            Bind<ISerializationConfiguration>().To<UXCDataJsonConvertersSerializationConfiguration>();

            Bind<ISerializationFactory, JsonSerializationFactory>().ToMethod(s => new JsonSerializationFactory(s.Kernel.GetAll<ISerializationConfiguration>()));
            Bind<ISerializationFactory, CsvSerializationFactory>().ToMethod(s => new CsvSerializationFactory(s.Kernel.GetAll<ISerializationConfiguration>()));

            Bind<DataIO>().ToMethod(s => new DataIO(s.Kernel.GetAll<ISerializationFactory>())).InSingletonScope();
        }


        private void BindProgramsSessionStep()
        {
            Bind<IProcessService>().To<ProcessService>().InSingletonScope();
        }
    }
}
