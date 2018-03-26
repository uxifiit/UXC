using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXC.Core.Devices;
using UXI.Common.Extensions;
using System.Threading;

namespace UXC.Devices.Adapters.Commands
{
    internal abstract class SyncDeviceCommand : DeviceCommandBase
    {
        protected SyncDeviceCommand(DeviceAction trigger, DeviceState targetState)
            : base(trigger, targetState) { }


        protected abstract CommandResult Execute(IDevice device);


        public sealed override Task<CommandResult> ExecuteAsync(IDevice device, CancellationToken cancellationToken)
        {
            return ConvertToAsync(Execute, device, cancellationToken);
        }


        private Task<CommandResult> ConvertToAsync(Func<IDevice, CommandResult> commandAction, IDevice device, CancellationToken cancellationToken)
        {
            Task<CommandResult> execution = Task.FromResult(CommandResult.NotApplied);
            if (cancellationToken.IsCancellationRequested == false)
            {
                var commandExecution = WrapTryCatch(() => commandAction(device), ex => CommandResult.Failed);

                if (device.Code.RunsOnMainThread)
                {
                    execution = Task.FromResult(commandExecution.Invoke());
                }
                else
                {
                    execution = Task.Run(commandExecution, cancellationToken);
                }
            }

            return execution;
        }


        private static Func<TResult> WrapTryCatch<TResult>(Func<TResult> func, Func<Exception, TResult> exceptionHandler) //, Action finallyHandler) 
        {
            return () =>
            {
                try
                {
                    TResult result = func.Invoke();
                    return result;
                }
                catch (Exception ex)
                {
                    return exceptionHandler.Invoke(ex);
                }
            };
        }
    }


    internal abstract class DeviceCommandBase : IDeviceCommand
    {
        protected DeviceCommandBase(DeviceAction trigger, DeviceState targetState)
        {
            TargetState = targetState;
            Action = trigger;
        }


        public DeviceState TargetState { get; }


        public DeviceAction Action { get; }


        public abstract Task<CommandResult> ExecuteAsync(IDevice device, CancellationToken cancellationToken);


        public virtual bool CanExecute(IDevice device)
        {
            return true;
        }


        protected CommandResult Result(bool value, CommandResult trueResult = CommandResult.Success, CommandResult falseResult = CommandResult.Failed)
        {
            return value ? trueResult : falseResult;
        }
    }
}
