using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.UI;

namespace GazeVisualization.ViewModels
{
    class FixationFilterSettingsViewModel : BindableBase
    {
        public FixationFilterSettingsViewModel()
        {
            Options = new List<FixationFilterStepOptionsViewModel>()
            {
                CreateOption(new FillInGapsOptions(), _fillInGaps.OnNext),
                CreateOption(new EyeSelectionOptions(), _eyeSelections.OnNext),
                CreateOption(new NoiseReductionOptions(), _noiseReduction.OnNext),
                CreateOption(new FixationsClassificationOptions(), _classification.OnNext),
                CreateOption(new MergeAdjacentFixationsOptions(), _merge.OnNext),
                CreateOption(new DiscardShortFixationsOptions(), _discard.OnNext),
            };
        }

        private static FixationFilterStepOptionsViewModel CreateOption<T>(T options, Action<T> onNextChange) where T : BaseOptions
        {
            var viewmodel = new FixationFilterStepOptionsViewModel(options);
            viewmodel.OptionsChanged += (option, isEnabled) => onNextChange.Invoke(isEnabled ? option as T : null);
            onNextChange(viewmodel.IsEnabled ? viewmodel.Options as T : null);
            return viewmodel;
        }


        public List<FixationFilterStepOptionsViewModel> Options { get; }


        private readonly ReplaySubject<FillInGapsOptions> _fillInGaps = new ReplaySubject<FillInGapsOptions>();
        public IObservable<FillInGapsOptions> FillInGapsOptions => _fillInGaps;


        private readonly ReplaySubject<EyeSelectionOptions> _eyeSelections = new ReplaySubject<EyeSelectionOptions>();
        public IObservable<EyeSelectionOptions> EyeSelectionOptions => _eyeSelections;


        private readonly ReplaySubject<NoiseReductionOptions> _noiseReduction = new ReplaySubject<NoiseReductionOptions>();
        public IObservable<NoiseReductionOptions> NoiseReductionOptions => _noiseReduction;


        private readonly ReplaySubject<FixationsClassificationOptions> _classification = new ReplaySubject<FixationsClassificationOptions>();
        public IObservable<FixationsClassificationOptions> FixationsClassificationOptions => _classification;


        private readonly ReplaySubject<MergeAdjacentFixationsOptions> _merge = new ReplaySubject<MergeAdjacentFixationsOptions>();
        public IObservable<MergeAdjacentFixationsOptions> MergeAdjacentFixationsOptions => _merge;


        private readonly ReplaySubject<DiscardShortFixationsOptions> _discard = new ReplaySubject<DiscardShortFixationsOptions>();
        public IObservable<DiscardShortFixationsOptions> DiscardShortFixationsOptions => _discard;
    }
}
