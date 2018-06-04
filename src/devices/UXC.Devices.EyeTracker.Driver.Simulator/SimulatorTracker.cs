using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UXC.Devices.EyeTracker.Calibration;
using UXC.Core.Data;
using UXI.Common;
using UXC.Core.Devices.Exceptions;
using UXI.Common.Extensions;
using UXI.SystemApi.Mouse;
using System.Reactive.Linq;
using UXI.SystemApi;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using UXI.SystemApi.Screen;
using UXC.Devices.EyeTracker.Driver.Simulator.Extensions;

namespace UXC.Devices.EyeTracker.Driver.Simulator
{
    class SimulatorTracker : DisposableBase, IEyeTrackerDriver
    {
        private readonly Random _random = new Random();
        private readonly ScreenParametersProvider _screen;
        private readonly MouseCoordinatesHook _hook;

        private readonly GazePointGenerator _dataGenerator = new GazePointGenerator();

        private readonly ReplaySubject<IObservable<GazeData>> _gazeSources = new ReplaySubject<IObservable<GazeData>>();
        private IObservable<GazeData> GazeSource => _gazeSources.Switch();

        private readonly SerialDisposable _recording = new SerialDisposable();

        public SimulatorTracker(ScreenParametersProvider screen, MouseCoordinatesHook hook)
        {
            _screen = screen;
            _hook = hook;
        }

        public string FamilyName => "UXI";

        public string Name => "Eye Tracker Simulator";
        public string ProductId => String.Empty;

        public float Framerate => 60;


        public event EventHandler<string> ConnectionError { add { } remove { } }
        public event GazeDataReceivedEventHandler GazeDataReceived;


        private TrackBoxCoords trackBox = new TrackBoxCoords();
        public TrackBoxCoords TrackBox
        {
            get { return trackBox; }
            private set
            {
                var previous = ObjectEx.GetAndReplace(ref trackBox, value);
                if (previous != value)
                {
                    TrackBoxChanged?.Invoke(this, value);
                }
            }
        }
        public event EventHandler<TrackBoxCoords> TrackBoxChanged;


        private DisplayArea displayArea = new DisplayArea(Point3.Default, Point3.Default, Point3.Default);
        public DisplayArea DisplayArea
        {
            get { return displayArea; }
            private set
            {
                var previous = ObjectEx.GetAndReplace(ref displayArea, value);
                if (previous != value)
                {
                    DisplayAreaChanged?.Invoke(this, value);
                }
            }
        }
        public event EventHandler<DisplayArea> DisplayAreaChanged;


        
        public void Connect()
        {
            try
            {
                var frequency = Observable.Interval(TimeSpan.FromMilliseconds(1000d / Framerate), NewThreadScheduler.Default);

                var cursorPositions = Observable.Create<Point>(s =>
                {
                    return frequency.Subscribe(_ =>
                    {
                        Point point;
                        if (_hook.TryGetCursorPosition(out point))
                        {
                            s.OnNext(point);
                        }
                    });
                });

                var simulatedGazePoints = Observable.Create<GazeData>(s =>
                {
                    ScreenInfo screen = null;

                    return cursorPositions.Subscribe(cursorPosition =>
                    {
                        GazeData gaze;

                        if ((screen != null || _screen.TryGetScreenInfo(cursorPosition.X, cursorPosition.Y, out screen))
                            && screen.Contains(cursorPosition))
                        {
                            Point2 point = new Point2
                                            (
                                                x: (cursorPosition.X - screen.Left) / (double)screen.Width,
                                                y: (cursorPosition.Y - screen.Top) / (double)screen.Height
                                            );

                            gaze = _dataGenerator.CreateValidData(point);
                        }
                        else
                        {
                            gaze = _dataGenerator.CreateInvalidData();
                        }

                        s.OnNext(gaze);
                    });
                });

                _gazeSources.OnNext(simulatedGazePoints);

                //AttachEventHandlers();
                //Framerate = 60;//_tracker.GetFrameRate();
                //TrackBox = _tracker.GetTrackBox()?.ToTrackBoxCoords();
                //DisplayArea = _tracker.GetXConfiguration()?.ToDisplayArea();
                //Calibration = _tracker.GetCalibration()?.ToCalibrationResult();
            }
            catch (Exception exception)
            {
                throw new ConnectionException("Simulator not found.", exception);
            }
        }

     
        public void Disconnect()
        {
            _gazeSources.OnNext(Observable.Empty<GazeData>());
            _recording.Disposable = Disposable.Empty;
        }


        public void StartTracking()
        {
            _recording.Disposable = GazeSource.Subscribe(gz => GazeDataReceived?.Invoke(this, gz));
        }


        public void StopTracking()
        {
            _recording.Disposable = Disposable.Empty;
        }

      
        public async Task AddCalibrationPointAsync(Point2 point)
        {
            List<CalibrationPointResult> calibration = _calibration.Value;

            var gazePoints = await GazeSource.Take(10).ToList();

            calibration.Add(new CalibrationPointResult
            (
                point, 
                gazePoints.Select(p => new CalibrationSample
                (
                    new CalibrationEyeSample(p.LeftEye.GazePoint2D, CalibrationSampleStatus.ValidAndUsedInCalibration), 
                    new CalibrationEyeSample(p.RightEye.GazePoint2D, CalibrationSampleStatus.ValidAndUsedInCalibration)
                ))
            ));
        }


        public Task<CalibrationResult> ComputeCalibrationAsync()
        {
            var calibration = new CalibrationResult(_calibration.Value, CalibrationStatus.Unknown);

            return Task.FromResult(calibration);
        }
    

        private Lazy<List<CalibrationPointResult>> _calibration = new Lazy<List<CalibrationPointResult>>();

   
        public Task StartCalibrationAsync()
        {
            _calibration.Value.Clear();

            _recording.Disposable = GazeSource.Subscribe();

            return Task.FromResult(true);
        }


        public Task StopCalibrationAsync()
        {
            _recording.Disposable = Disposable.Empty;

            return Task.FromResult(true);
        }


        public Task SetCalibrationAsync(CalibrationData calibrationData)
        {
            calibrationData.ThrowIf(c => c.DeviceFamilyName != FamilyName, nameof(calibrationData), "Calibration data is for the different family of devices.");

            IEnumerable<CalibrationPointResult> points = CalibrationSerializer.Deserialize(calibrationData.Data);

            var calibration = _calibration.Value;
            calibration.Clear();
            calibration.AddRange(points);

            return Task.FromResult(true);
        }


        public CalibrationData GetCalibration()
        {
            return _calibration.IsValueCreated
                 ? new CalibrationData(FamilyName, Name, CalibrationSerializer.Serialize(_calibration.Value))
                 : null;
        }


        //private int i = 0;
        //private int max = 1000;
        //private int step = 1;

        /// <summary>
        /// Maps Gaze Data from the simulator to the <see cref="UXC.Core.Data.GazeData"/> class.
        /// </summary>
        /// <param name="gazeDataItem">Gaze Data from the tracker</param>
        /// <returns>Gaze Data in our format</returns>
        public GazeData CreateGazeData(Point2 relativeCursorPosition, bool isValidPosition)
        {
            Func<double, double> deviateX = x => x;// + (0.5 - _random.NextDouble()) / 10;
            Func<double, double> deviateY = y => y;// + (0.5 - _random.NextDouble()) / 100;


            //i = (i + step) % max;
            Func<double> randomRelativePosition = () => 0.5; //(double)i / max;
                                                             // - (_random.NextDouble() / 10);

            var validity = isValidPosition ? GazeDataValidity.Both : GazeDataValidity.None;

            var gaze = new GazeData
            (
                validity,
                new EyeGazeData
                (
                    validity.GetLeftEyeValidity(),
                    new Point2(deviateX(relativeCursorPosition.X), deviateY(relativeCursorPosition.Y)),
                    new Point3(randomRelativePosition(), randomRelativePosition(), randomRelativePosition()),
                    new Point3(randomRelativePosition(), randomRelativePosition(), randomRelativePosition()),
                    new Point3(randomRelativePosition(), randomRelativePosition(), randomRelativePosition()),
                    randomRelativePosition()
                ),
                new EyeGazeData
                (
                    validity.GetRightEyeValidity(),
                    new Point2(deviateX(relativeCursorPosition.X), deviateY(relativeCursorPosition.Y)),
                    new Point3(randomRelativePosition(), randomRelativePosition(), randomRelativePosition()),
                    new Point3(randomRelativePosition(), randomRelativePosition(), randomRelativePosition()),
                    new Point3(randomRelativePosition(), randomRelativePosition(), randomRelativePosition()),
                    randomRelativePosition()
                ),
                DateTime.Now.Ticks,
                DateTime.Now
            );

            return gaze;
        }


   


        #region IDisposable Members

        private bool _disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    Disconnect();
                    _recording.Dispose();
                    _gazeSources.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
