using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Devices.Adapters;
using UXC.Observers;
using UXC.Core.Data.Conversion.GazeToolkit;

namespace GazeVisualization.Observers
{
    class GazeObserver : IDeviceObserver
    {
        private readonly ReplaySubject<IObservable<UXI.GazeToolkit.GazeData>> _gazeStreams = new ReplaySubject<IObservable<UXI.GazeToolkit.GazeData>>(1);

        public GazeObserver()
        {
            Clear();
            RawGaze.Subscribe(g => System.Diagnostics.Debug.WriteLine(g.ToString()));
        }

        public IObservable<IObservable<UXI.GazeToolkit.GazeData>> RawGaze => _gazeStreams;

        private void Clear()
        {
            _gazeStreams.OnNext(Observable.Empty<UXI.GazeToolkit.GazeData>());
        }

        public IDisposable Connect(IObservableDevice device)
        {
            var gazeData = device.Data
                                 .OfType<GazeData>()
                                 .Select(g => g.ToToolkit())
                                 .ObserveOnDispatcher();

            _gazeStreams.OnNext(gazeData);

            return Disposable.Create(Clear);
        }

        public bool IsDeviceSupported(DeviceType type)
        {
            return DeviceType.Physiological.EYETRACKER.Equals(type);
        }
    }
}
