using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using UXC.Core;
using UXC.Core.ViewModels;
using UXC.Sessions.Models;
using UXC.Sessions.Models.Design;
using UXC.Sessions.ViewModels;
using UXC.Sessions.ViewModels.Timeline.Factories;
using UXI.Common.UI;

namespace UXC.Sessions
{
    public class SessionsUIModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IViewModelFactory>().To<DefaultTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<InstructionsTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<LaunchProgramTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<ShowDesktopTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<WelcomeTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<QuestionaryTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<ChooseQuestionAnswerViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<WriteQuestionAnswerViewModelFactory>().InSingletonScope();

#if DEBUG
            if (DesignTimeHelper.IsDesignTime)
            {
                Bind<ISessionRecordingsDataSource>().To<DesignSessionRecordingsDataSource>().InSingletonScope();
            }
            else
            {
#endif
                Bind<ISessionRecordingsDataSource>().To<SessionRecordingsDataSource>().InSingletonScope();
#if DEBUG
            }
#endif

            Bind<SessionRecordingViewModelFactory>().ToSelf().InSingletonScope();
            Bind<SessionsViewModel>().ToSelf().InSingletonScope();
            Bind<SessionDefinitionsViewModel>().ToSelf();

            Bind<SessionRecordingsDataViewModel>().ToSelf().InSingletonScope();
        }
    }
}
