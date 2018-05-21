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

            if (settings.ShowContinue)
            {
                ContinueCommand = new RelayCommand(() => Complete());
            }
        }


        public string Instructions { get; }


        public Visibility ContinueButtonVisibility => _settings.ShowContinue ? Visibility.Visible : Visibility.Collapsed;


        public ICommand ContinueCommand { get; } = NullCommand.Instance;


        public override void Execute(SessionRecordingViewModel recording)
        {
        }


        public override SessionStepResult Complete()
        {
            var result = SessionStepResult.Successful; // TODO add the bounding box of instructions

            OnCompleted(result);

            return result;
        }
    }
}
