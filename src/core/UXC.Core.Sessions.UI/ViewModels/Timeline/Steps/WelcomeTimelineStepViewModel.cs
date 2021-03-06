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
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using UXC.Core.Managers;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;
using UXC.Sessions.ViewModels.Timeline;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels.Timeline
{
    public class WelcomeTimelineStepViewModel : QuestionaryTimelineStepViewModel
    {
        private readonly WelcomeActionSettings _settings;

        private SessionRecordingViewModel _recording;


        public WelcomeTimelineStepViewModel(WelcomeActionSettings settings, IAdaptersManager adapters, ViewModelResolver resolver)
            : base(settings, resolver)
        {
            _settings = settings;
            Description = _settings.Description?.Lines != null && _settings.Description.Lines.Any()
                        ? String.Join(Environment.NewLine, _settings.Description.Lines)
                        : String.Empty;

            string label = settings.StartButtonLabel?.Trim();
            StartButtonLabel = String.IsNullOrWhiteSpace(label) ? null : label;
        }


        public Visibility DescriptionVisibility => _settings.HideDescription ? Visibility.Collapsed : Visibility.Visible;


        public string Description { get; }


        public string StartButtonLabel { get; }


        public Visibility DevicesListVisibility => _settings.HideDevices ? Visibility.Collapsed : Visibility.Visible;


        public override void Execute(SessionRecordingViewModel recording)
        {
            base.Execute(recording);

            if (String.IsNullOrWhiteSpace(Title))
            {
                Title = recording.Recording.Definition.Project;
            }

            OnPropertyChanged(nameof(Title));
        }


        public override SessionStepResult Complete()
        {
            return base.Complete();
        }
    }
}
