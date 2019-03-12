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
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;
using UXC.Core.Common.Commands;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;
using UXI.Common.UI;
using UXC.Sessions.Common.Helpers;

namespace UXC.Sessions.ViewModels.Timeline
{
    public class InstructionsTimelineStepViewModel : ContentTimelineStepViewModelBase
    {
        private readonly InstructionsActionSettings _settings;

        public InstructionsTimelineStepViewModel(InstructionsActionSettings settings)
            : base(settings)
        {
            _settings = settings;
            Instructions = _settings.Instructions.Lines != null && _settings.Instructions.Lines.Any()
                         ? String.Join(Environment.NewLine, _settings.Instructions.Lines)
                         : String.Empty;

            Title = _settings.Title?.Trim() ?? String.Empty;

            if (settings.ShowContinue)
            {
                ContinueCommand = new RelayCommand(() => Complete());
                string label = settings.ContinueButtonLabel?.Trim();
                ContinueButtonLabel = String.IsNullOrWhiteSpace(label) ? null : label;
            }
        }


        private string instructions;
        public string Instructions
        {
            get { return instructions; }
            private set { Set(ref instructions, value); }
        }


        private string title;
        public string Title
        {
            get { return title; }
            private set { Set(ref title, value); }
        }


        public Visibility ContinueButtonVisibility => _settings.ShowContinue ? Visibility.Visible : Visibility.Collapsed;


        public ICommand ContinueCommand { get; } = NullCommand.Instance;


        public string ContinueButtonLabel { get; } = null;


        public override void Execute(SessionRecordingViewModel recording)
        {
            if (_settings.Parameters != null && _settings.Parameters.Any())
            {
                Instructions = SessionRecordingSettingsHelper.FillParameters(Instructions, _settings.Parameters, recording.Recording.Settings);
                Title = SessionRecordingSettingsHelper.FillParameters(Title, _settings.Parameters, recording.Recording.Settings);
            }
        }


        public override SessionStepResult Complete()
        {
            var result = SessionStepResult.Successful; // TODO add the bounding box of instructions

            OnCompleted(result);

            return result;
        }
    }
}
