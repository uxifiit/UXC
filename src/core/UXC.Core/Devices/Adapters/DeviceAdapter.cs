using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;
using UXC.Core.Devices;
using UXC.Core.Devices.Calibration;
using UXC.Models;
using UXC.Devices.Adapters.Commands;
using System.Threading;
using Stateless;
using UXI.Common;
using UXC.Core.Configuration;
using System.Diagnostics;

namespace UXC.Devices.Adapters
{
    public class DeviceAdapter : DisposableBase, IDeviceAdapter
    {
        private readonly IDevice _device;
        private readonly StateMachine<DeviceState, DeviceAction> _states;
        private readonly DeviceCommands _commands;
        private static IEnumerable<DeviceAction> InternalTriggers
        {
            get
            {
                yield return DeviceAction.Error;
            }
        }

        public DeviceAdapter(IDevice device)
        {
            _device = device.ThrowIfNull(nameof(device));
            _device.ConnectionError += Device_ConnectionError;

            _commands = new DeviceCommands(this);

            Calibration = (device as ICalibrate) ?? NullDeviceCalibration.Instance;

            Configurator = CreateConfigurator(device);

            Observables = DeviceObservables.CreateForDevice(this, _device);

            _states = CreateStateMachine();
        }

        private void Device_ConnectionError(IDevice device, Exception exception)
        {
            // TODO LOG Exception

            if (_states.CanFire(DeviceAction.Error))
            {
                // stop recording
                // cancel execution?
                _states.Fire(DeviceAction.Error);
            }
        }

        private StateMachine<DeviceState, DeviceAction> CreateStateMachine()
        {
            var states = new StateMachine<DeviceState, DeviceAction>(() => State, s => State = s);

            states.Configure(DeviceState.Disconnected)
                  .Permit(DeviceAction.Connect, DeviceState.Connected);

            states.Configure(DeviceState.Connected)
                  .Permit(DeviceAction.Disconnect, DeviceState.Disconnected)
                  .Permit(DeviceAction.StartRecording, DeviceState.Recording)
                  .Permit(DeviceAction.Error, DeviceState.Error);

            states.Configure(DeviceState.Recording)
                  .Permit(DeviceAction.StopRecording, DeviceState.Connected) 
                  .Permit(DeviceAction.Error, DeviceState.Error);

            states.Configure(DeviceState.Error)
                  .Permit(DeviceAction.Disconnect, DeviceState.Disconnected);

            return states;
        }


        private static IConfigurator CreateConfigurator(IDevice device)
        {
            if (device is IConfigurable)
            {
                return new Configurator((IConfigurable)device);
            }
            return NullConfigurator.Instance;
        }

   

        public IEnumerable<DeviceAction> AvailableForwardActions // use in VM
        {
            get
            {
                var triggers = _states.PermittedTriggers.Except(InternalTriggers).ToArray();
                return _commands.Forward
                                .Where(c => triggers.Contains(c.Action))
                                .Select(c => c.Action);
            }
        }


        public IEnumerable<DeviceAction> AvailableBackwardActions
        {
            get
            {
                var triggers = _states.PermittedTriggers.Except(InternalTriggers).ToArray();
                return _commands.Backward
                                .Where(c => triggers.Contains(c.Action))
                                .Select(c => c.Action);
            }
        }


        //public bool IsReadyForRecording => Calibration.CanCalibrate == false || (Calibration.CanCalibrate && Calibration.RequiresCalibration == false);


        public DeviceCode Code => _device.Code;


        private DeviceState state = DeviceState.Disconnected;
        public DeviceState State
        {
            get { return state; }
            private set
            {
                DeviceState previous = state;
                if (previous != value)
                {
                    state = value;
                    StateChanged?.Invoke(this, new DeviceStateChangedEventArgs(value, previous));
                }
            }
        }

        public event DeviceStateChangedEventHandler StateChanged;


        public ICalibrate Calibration { get; }

        public IConfigurator Configurator { get; }

        public IObservableDevice Observables { get; }



        private DeviceCommandExecution execution = DeviceCommandExecution.Default;
        public DeviceCommandExecution RecentCommandExecution
        {
            get { return execution; }
            private set
            {
                if (execution != value)
                {
                    execution = value;
                    CommandExecutionChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<DeviceCommandExecution> CommandExecutionChanged;


        public bool TryGoToState(DeviceState targetState, CancellationToken cancellationToken, out Task<CommandResult> execution)
        {
            var currentState = State;
            execution = null;
            if (RecentCommandExecution.IsWorking == false && currentState != targetState)
            {
                IEnumerable<IDeviceCommand> commands = Enumerable.Empty<IDeviceCommand>();
                var availableTriggers = _states.PermittedTriggers.ToArray();

                if (currentState == DeviceState.Error)
                {   // shortcut instead of double routing to target state first through disconnect after error and then forward to the target state.
                    commands = _commands.Backward
                                        .SkipWhile(c => availableTriggers.Contains(c.Action) == false)
                                        .Concat(_commands.Forward.TakeUntil(c => c.TargetState == targetState));
                }
                else if (currentState < targetState)
                {
                    commands = _commands.Forward
                                        .SkipWhile(c => availableTriggers.Contains(c.Action) == false)
                                        .TakeUntil(c => c.TargetState == targetState);
                }
                else if (currentState > targetState)
                {
                    commands = _commands.Backward
                                        .SkipWhile(c => availableTriggers.Contains(c.Action) == false)
                                        .TakeUntil(c => c.TargetState == targetState);
                }

                execution = ApplyCommandsAsync(commands, cancellationToken);

                if (execution != null)
                {
                    RecentCommandExecution = new DeviceCommandExecution(execution, targetState);
                    return true;
                }
            }

            return false;
        }


        private bool CanApplyCommand(IDeviceCommand command)
        {
            return _states.CanFire(command.Action) && command.CanExecute(_device);
        }   


        private async Task<CommandResult> ApplyCommandsAsync(IEnumerable<IDeviceCommand> commands, CancellationToken cancellationToken)
        {
            var result = CommandResult.NotApplied;

            foreach (var command in commands)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return CommandResult.Cancelled;
                }

                if (CanApplyCommand(command))
                {
                    result = await command.ExecuteAsync(_device, cancellationToken);

                    if (result == CommandResult.Success 
                        && _states.CanFire(command.Action))
                    {
                        _states.Fire(command.Action);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return result;
        }


        public bool CanExecuteAction(DeviceAction action)
        {
            IDeviceCommand command = null;
            return _commands.TryGet(c => c.Action == action, out command)
                && CanApplyCommand(command);
        }


        public async Task<CommandResult> ExecuteActionAsync(DeviceAction action, CancellationToken cancellationToken)
        {
            var result = CommandResult.NotApplied;

            IDeviceCommand command;

            if (_commands.TryGet(c => c.Action == action, out command)
                && CanApplyCommand(command))
            {
                var execution = ApplyCommandsAsync(new[] { command }, cancellationToken);

                if (execution != null)
                {
                    RecentCommandExecution = new DeviceCommandExecution(execution, command.TargetState);

                    result = await execution;
                }
            }

            return result;
        }



        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    if (State.HasFlag(DeviceState.Connected))
                    {
                        _device.DisconnectDevice();
                    }
                    _device.ConnectionError -= Device_ConnectionError;
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}


