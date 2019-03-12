/**
 * UXC.Devices.EyeTracker
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
using Stateless;
using UXC.Core.Common.Events;
using UXC.Core.Devices.Calibration;
using UXC.Core.Data;
using UXI.Common.Extensions;
using System.Collections.ObjectModel;

namespace UXC.Devices.EyeTracker.Calibration
{
    public class EyeTrackerCalibrator : ICalibrator, IEyeTrackerCalibrationExecution
    {
        private enum CalibratorAction
        {
            Prepare,
            Restore,
            Start,
            Continue,
            Stop,
            Finish,
            Submit,
            Retry,
            Cancel
        }


        private readonly IEyeTrackerDriver _tracker;
        private readonly List<CalibrationResult> _calibrations = new List<CalibrationResult>();
        private readonly StateMachine<CalibratorState, CalibratorAction> _states;
        private readonly List<Point2> _calibratedPoints = new List<Point2>();

        private int _pointsLeft = 0;
        private StateMachine<CalibratorState, CalibratorAction>.TriggerWithParameters<Point2> _continueTrigger;

        internal EyeTrackerCalibrator(EyeTrackerDevice device, IEyeTrackerDriver tracker)
        {
            _tracker = tracker;
            State = CalibratorState.None;
            Target = device;
            DeviceInfo = device.DeviceInfo;
            HeadPositioning = new HeadPositioning(tracker);
            Calibrations = new ReadOnlyCollection<CalibrationResult>(_calibrations);

            _states = CreateStateMachine();
        }

        private StateMachine<CalibratorState, CalibratorAction> CreateStateMachine()
        {
            var states = new StateMachine<CalibratorState, CalibratorAction>(() => State, state => State = state);

            _continueTrigger = states.SetTriggerParameters<Point2>(CalibratorAction.Continue);

            states.Configure(CalibratorState.None)
                  .Permit(CalibratorAction.Prepare, CalibratorState.Preparation)
                  .Permit(CalibratorAction.Cancel, CalibratorState.Canceled);

            states.Configure(CalibratorState.Preparation)
                  .OnEntry(HeadPositioning.Start)
                  .Permit(CalibratorAction.Start, CalibratorState.Running)
                  .Permit(CalibratorAction.Cancel, CalibratorState.Canceled)
                  .Permit(CalibratorAction.Restore, CalibratorState.Completed)
                  .OnExit(HeadPositioning.Stop);

            states.Configure(CalibratorState.Running)
                  .InternalTransitionAsync(_continueTrigger, (point, _) => OnNextPointAsync(point))
                  .PermitIf(CalibratorAction.Finish, CalibratorState.Finished, () => _pointsLeft <= 0)
                  .Permit(CalibratorAction.Cancel, CalibratorState.Preparation)
                  .OnExitAsync(OnFinishedAsync);

            states.Configure(CalibratorState.Finished)
                  .Permit(CalibratorAction.Submit, CalibratorState.Completed)
                  .Permit(CalibratorAction.Retry, CalibratorState.Preparation)
                  .Permit(CalibratorAction.Cancel, CalibratorState.Canceled);

            states.Configure(CalibratorState.Completed);

            states.Configure(CalibratorState.Canceled)
                  .SubstateOf(CalibratorState.None);

            return states;
        }

   
        public bool CanPrepare() => _states.CanFire(CalibratorAction.Prepare);
        public void Prepare() => _states.Fire(CalibratorAction.Prepare);


        public bool CanStart(CalibrationPlan plan) => _states.CanFire(CalibratorAction.Start)
            && plan != null
            && plan.Length > 0
            && plan.All(p => p.X > 0d && p.Y < 1d);

        public async Task StartAsync(CalibrationPlan plan)
        {
            _pointsLeft = plan.Length;
            _calibratedPoints.Clear();

            await _tracker.StartCalibrationAsync();

            _states.Fire(CalibratorAction.Start);
        }


        public bool CanRestore(CalibrationData calibrationData) => _states.CanFire(CalibratorAction.Restore)
            && _tracker.FamilyName.Equals(calibrationData.DeviceFamilyName, StringComparison.CurrentCultureIgnoreCase);
       //     && _tracker.Name.Equals(calibrationData.DeviceName, StringComparison.CurrentCultureIgnoreCase);

        public async Task RestoreAsync(CalibrationData calibrationData)
        {
            if (_states.CanFire(CalibratorAction.Restore))
            {
                await _tracker.StartCalibrationAsync();

                await _tracker.SetCalibrationAsync(calibrationData);

                await _tracker.StopCalibrationAsync();

                _states.Fire(CalibratorAction.Restore);
            }
        }


        public bool CanFinish() => _states.CanFire(CalibratorAction.Finish);

        public async Task<CalibrationExecutionReport> FinishAsync()
        {
            if (_states.CanFire(CalibratorAction.Finish))
            {
                CalibrationExecutionReport report = null;
                if (_pointsLeft <= 0)
                {
                    var result = await _tracker.ComputeCalibrationAsync();
                    var data = _tracker.GetCalibration();
                    var plan = new CalibrationPlan(_calibratedPoints);
                    report = new CalibrationExecutionReport(plan, result, data);

                    _calibrations.Add(result);
                    CalibrationFinished?.Invoke(this, result);
                }
     
                await _states.FireAsync(CalibratorAction.Finish);

                return report;
            }
            else
            {
                throw new InvalidOperationException($"Cannot finish calibration in the current state of Calibrator: {State.ToString()}");
            }
        }


        private async Task OnFinishedAsync()
        {
            await _tracker.StopCalibrationAsync();
        }


        public bool CanContinue(Point2 point) => _states.CanFire(_continueTrigger.Trigger);
        public Task ContinueAsync(Point2 point) => _states.FireAsync(_continueTrigger, point);

        private Task OnNextPointAsync(Point2 point)
        {
            _pointsLeft -= 1;
            _calibratedPoints.Add(point);

            return _tracker.AddCalibrationPointAsync(point);
        }


        public bool CanRetry() => _states.CanFire(CalibratorAction.Retry);
        public void Retry() => _states.Fire(CalibratorAction.Retry);


        public bool CanSubmit() => _states.CanFire(CalibratorAction.Submit);
        public void Submit() => _states.Fire(CalibratorAction.Submit);


        public Task CancelAsync() => _states.FireAsync(CalibratorAction.Cancel);


        public ICalibrate Target { get; }


        public EyeTrackerDeviceInfo DeviceInfo { get; }


        private CalibratorState state;
        public CalibratorState State
        {
            get
            {
                return state;
            }
            private set
            {
                if (state != value)
                {
                    state = value;
                    StateChanged?.Invoke(this, value);
                }
            }
        }

        public event CalibratorStateChangedEventHandler StateChanged;


        public HeadPositioning HeadPositioning { get; }


        public IReadOnlyCollection<CalibrationResult> Calibrations { get; }


        public event EventHandler<CalibrationResult> CalibrationFinished;
    }
}
