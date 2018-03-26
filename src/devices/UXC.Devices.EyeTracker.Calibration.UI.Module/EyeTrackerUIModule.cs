using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.ViewModels;
using UXC.Devices.EyeTracker.ViewModels;
using UXC.Devices.EyeTracker.Models;

namespace UXC.Devices.EyeTracker.Calibration.UI.Module
{
    public class EyeTrackerUIModule : NinjectModule
    {
        public override void VerifyRequiredModulesAreLoaded()
        {
            base.VerifyRequiredModulesAreLoaded();
            // TODO add check for eye tracker
        }


        public override void Load()
        {
            Bind<ICalibrationProfilesService>().To<CalibrationProfilesService>();

            Bind<IViewModelFactory>().To<CalibratorViewModelFactory>().InSingletonScope();

            Bind<ISettingsSectionViewModel>().To<EyeTrackerSettingsSectionViewModel>().InSingletonScope();
        }
    }
}
