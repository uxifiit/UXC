using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Devices.Adapters;
using UXC.Devices.EyeTracker.Utils.Filters;
using UXC.Devices.EyeTracker.Utils.Filters.Selectors;
using UXC.Devices.EyeTracker.Utils.Filters.Smoothing;
using UXC.Observers;

namespace Debug.Fixations.Observers
{
    class FixationsObserver : IDeviceObserver
    {
        static readonly VelocityThresholdFixationFilter filter = new VelocityThresholdFixationFilter();

        public IDisposable Connect(IObservableDevice device)
        {
            var gazeData = device.Data.OfType<GazeData>();

            var dataGapsFilledIn = filter.FillInGaps(gazeData, TimeSpan.FromMilliseconds(75));
            var dataSelected = filter.SelectEye(dataGapsFilledIn, AverageSelector.Instance);
            var dataSmoothed = filter.ReduceNoise(dataSelected, new ExponentialSmoothingFilter(0.3));
            var dataVelocity = filter.CalculateVelocities(dataSmoothed, TimeSpan.FromMilliseconds(20), 60);
            var fixations = filter.ClassifyMovements(dataVelocity, 30);

            return fixations.ObserveOnDispatcher()
                            .Subscribe(f => System.Diagnostics.Debug.WriteLine($"{f.MovementType.ToString()}: {f.Position.X},{f.Position.Y} [{f.Duration.TotalMilliseconds}]"));             
        }

        public bool IsDeviceSupported(DeviceType type)
        {
            return DeviceType.Physiological.EYETRACKER.Equals(type);
        }
    }
}
