/**
 * UXC.Plugins.Sessions.Fixations
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
using UXC.Core.Data.Serialization;
using UXC.Core.Modules;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Executors;
using UXC.Sessions.ViewModels.Timeline;

namespace UXC.Plugins.Sessions.Fixations.ViewModels
{
    class FixationFilterTimelineStepViewModelFactory : IViewModelFactory
    {
        private readonly IInstanceResolver _resolver;

        public FixationFilterTimelineStepViewModelFactory(IInstanceResolver resolver)
        {
            _resolver = resolver;
        }

        public Type SourceType { get; } = typeof(FixationFilterActionSettings);

        public Type ViewModelType { get; } = typeof(ITimelineStepViewModel);

        public object Create(object source)
        {
            return new ExecutedTimelineStepViewModel((FixationFilterActionSettings)source, new FixationFilterActionExecutor(_resolver.GetAll<IDataSerializationFactory>()), _resolver.Get<ViewModelResolver>());
        }
    }
}
