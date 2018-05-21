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

            BindDataJsonSerialization();
            BindLocalSessionDefinitions();

            RegisterTimelineSteps();
        }


        private void RegisterTimelineSteps()
        {
            SessionStepActionSettings.Register(typeof(ShowDesktopActionSettings));
            SessionStepActionSettings.Register(typeof(LaunchProgramActionSettings));
            SessionStepActionSettings.Register(typeof(QuestionaryActionSettings));
            SessionStepActionSettings.Register(typeof(ChooseQuestionAnswerActionSettings));
            SessionStepActionSettings.Register(typeof(WriteQuestionAnswerActionSettings));
            SessionStepActionSettings.Register(typeof(InstructionsActionSettings));
        }


        private void BindLocalSessionDefinitions()
        {
            Bind<ILocalSessionDefinitionsService>().To<LocalSessionDefinitionsService>().InSingletonScope();
        }


        private void BindDataJsonSerialization()
        {
            foreach (var converter in DataJsonConverters.Converters)
            {
                Bind<Newtonsoft.Json.JsonConverter>().ToConstant(converter)
                                                     .WhenInjectedExactlyInto<JsonSerializationFactory>();
            }

            Bind<IDataSerializationFactory>().To<JsonSerializationFactory>().InSingletonScope();
        }


        //private void BindCsvSerialization()
        //{
        //    // add other class maps

        //    foreach (var classMap in DataClassMaps.Maps)
        //    {
        //        Bind<CsvHelper.Configuration.ClassMap>().ToConstant(classMap).WhenInjectedExactlyInto<CsvSerializationFactory>();
        //    }

        //    Bind<IDataSerializationFactory>().To<CsvSerializationFactory>().InSingletonScope();
        //}
    }
}
