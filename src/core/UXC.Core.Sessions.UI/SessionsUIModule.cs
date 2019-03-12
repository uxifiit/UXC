/**
 * UXC.Core.Sessions.UI
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
using UXC.Core;
using UXC.Core.ViewModels;
using UXC.Sessions.Models;
using UXC.Sessions.Models.Design;
using UXC.Sessions.ViewModels;
using UXC.Sessions.ViewModels.Timeline.Factories;
using UXC.Sessions.ViewModels.Timeline.Preparation;
using UXI.Common.UI;

namespace UXC.Sessions
{
    public class SessionsUIModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IViewModelFactory>().To<DefaultTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<InstructionsTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<ImageTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<CloseProgramTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<LaunchProgramTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<ShowDesktopTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<WelcomeTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<QuestionaryTimelineStepViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<ChooseAnswerQuestionViewModelFactory>().InSingletonScope();
            Bind<IViewModelFactory>().To<WriteAnswerQuestionViewModelFactory>().InSingletonScope();

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

            Bind<IImageService>().To<ImageService>().InSingletonScope();
            Bind<ITimelineStepPreparation>().To<ImageTimelineStepPreparation>().InSingletonScope();

            Bind<TimelinePreparation>().ToSelf().InSingletonScope();

            Bind<SessionRecordingViewModelFactory>().ToSelf().InSingletonScope();
            Bind<SessionsViewModel>().ToSelf().InSingletonScope();
            Bind<SessionDefinitionsViewModel>().ToSelf();

            Bind<SessionRecordingsDataViewModel>().ToSelf().InSingletonScope();
        }
    }
}
