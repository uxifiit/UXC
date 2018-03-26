using System;
using System.Collections.Generic;
using UXC.Devices.Adapters.Commands;
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Models;
using System.Threading.Tasks;
using System.Threading;
using UXC.Core.Devices.Calibration;
using UXC.Core.Configuration;

namespace UXC.Devices.Adapters
{
    public interface IDeviceAdapter : IDisposable
    {
        DeviceCode Code { get; }
        DeviceState State { get; }

        ICalibrate Calibration { get; }
        IConfigurator Configurator { get; }
        IObservableDevice Observables { get; }


        // May move the following members to some DeviceControlAdapter
        DeviceCommandExecution RecentCommandExecution { get; }

        event EventHandler<DeviceCommandExecution> CommandExecutionChanged;

        bool TryGoToState(DeviceState targetState, CancellationToken cancellationToken, out Task<CommandResult> execution);

        bool CanExecuteAction(DeviceAction trigger);
        Task<CommandResult> ExecuteActionAsync(DeviceAction action, CancellationToken cancellationToken);

        IEnumerable<DeviceAction> AvailableForwardActions { get; }
        IEnumerable<DeviceAction> AvailableBackwardActions { get; }

        event DeviceStateChangedEventHandler StateChanged;
    }

    public delegate void DeviceStateChangedEventHandler(IDeviceAdapter device, DeviceStateChangedEventArgs args);

}
