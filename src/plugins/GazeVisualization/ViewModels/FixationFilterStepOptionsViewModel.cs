using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.UI;

namespace GazeVisualization.ViewModels
{
    delegate void FixationFilterStepOptionChangedHandler(BaseOptions options, bool isEnabled);

    class FixationFilterStepOptionsViewModel : BindableBase
    {
        public FixationFilterStepOptionsViewModel(BaseOptions options)
        {
            Options = options;
            options.OptionsChanged += Options_OptionsChanged;
        }


        private void Options_OptionsChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Options));
            if (isEnabled)
            {
                OptionsChanged?.Invoke(Options, isEnabled);
            }
        }


        public BaseOptions Options { get; }

        public event FixationFilterStepOptionChangedHandler OptionsChanged;


        private bool isEnabled = false;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (Set(ref isEnabled, value))
                {
                    OptionsChanged?.Invoke(Options, value);
                }
            }
        }
    }
}
