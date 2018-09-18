using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UXC.Core.ViewModels;
using UXC.Sessions.ViewModels.Timeline;
using UXI.Common.Helpers;

namespace UXC.Sessions.Views.Timeline
{
    /// <summary>
    /// Interaction logic for EyeTrackerValidationTimelineStepControl.xaml
    /// </summary>
    public partial class EyeTrackerValidationTimelineStepControl : UserControl
    {
        private bool isRunning = false;
        private EyeTrackerValidationTimelineStepViewModel _validation;

        public EyeTrackerValidationTimelineStepControl()
        {
            this.Loaded += Control_Loaded;
            InitializeComponent();

        }


        async void Control_Loaded(object sender, RoutedEventArgs e)
        {
            if (isRunning == false)
            {
                isRunning = true;
                _validation = DataContext as EyeTrackerValidationTimelineStepViewModel;

                await RunCalibrationAsync();
            }
        }


        private async Task RunCalibrationAsync()
        {
            var validation = _validation;
            try
            {
                VisualStateManager.GoToElementState((FrameworkElement)this.Content, nameof(IntroductionState), true);

                await Task.Delay(validation.InstructionsDuration ?? TimeSpan.FromSeconds(3)); // TODO REFACTOR InstructionsDuration -> propdp

                VisualStateManager.GoToElementState((FrameworkElement)this.Content, nameof(ValidationProcessState), true);

                await Task.Delay(300); // wait till the instrucstions fade out completes

                validation?.Animation?.Start();
            }
            catch (Exception)
            {
                // TODO log exception
                // TODO 10/09/2016 add error
                //ErrorWindow.Show(ex);
            }
        }


        private void pointControl_TargetPointReached(object sender, Core.Controls.PointReachedEventArgs e)
        {
            //var control = (FrameworkElement)sender;
            var timeline = (EyeTrackerValidationTimelineStepViewModel)this.DataContext;
            var animation = timeline.Animation;
            if (animation != null)
            {
                animation.CompletePoint(e.Point);
            }
        }
    }
}
