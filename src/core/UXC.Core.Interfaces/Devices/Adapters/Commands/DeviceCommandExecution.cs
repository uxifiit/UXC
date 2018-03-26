using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UXC.Core.Devices;

namespace UXC.Devices.Adapters.Commands
{
    public class DeviceCommandExecution
    {
        public static readonly DeviceCommandExecution Default = new DeviceCommandExecution(Task.FromResult(CommandResult.NotApplied), DeviceState.Disconnected);

        public DeviceCommandExecution(Task<CommandResult> executionTask, DeviceState targetState)
        {
            ExecutionTask = executionTask.ContinueWith(OnCompleted);
            TargetState = targetState;
        }

        public Task<CommandResult> ExecutionTask { get; }

        public DeviceState TargetState { get; }


        public bool IsWorking => ExecutionTask.Status == TaskStatus.Running;
                              //&& ExecutionTask.Status != TaskStatus.Canceled
                              //&& ExecutionTask.Status != TaskStatus.Faulted;


        private CommandResult OnCompleted(Task<CommandResult> task)
        {
            Completed?.Invoke(this, task.Result);
            return task.Result;
        }

        public event EventHandler<CommandResult> Completed;
    }
}




        //public static DeviceCommandExecution Create
        //(
        //    IDevice device,
        //    DeviceState state,
        //    IDeviceCommand command,
        //    bool runOnBackground,
        //    CancellationToken cancellationToken
        //)
        //{
        //    Task<CommandResult> execution = Task.FromResult(CommandResult.NotApplied);

        //    Func<IDevice, CommandResult> commandExecute = null;
        //    Func<CommandResult> commandExecution = null;

        //    if (cancellationToken.IsCancellationRequested == false
        //        && CanExecuteCommand(command, state, out commandExecute))
        //    {
        //        commandExecution = WrapTryCatch(() => commandExecute(device), ex => CommandResult.Failed);
            
        //        if (runOnBackground)
        //        {
        //            execution = Task.Run(commandExecution, cancellationToken);
        //        }
        //        else
        //        {
        //            var tcs = new TaskCompletionSource<CommandResult>();
        //            execution = tcs.Task;
        //            tcs.TrySetResult(commandExecution.Invoke());
        //        }
        //    }

        //    return new DeviceCommandExecution(execution, command.TargetState);
        //}

        //private static Func<TResult> WrapTryCatch<TResult>(Func<TResult> func, Func<Exception, TResult> exceptionHandler) //, Action finallyHandler) 
        //{
        //    return () =>
        //    {
        //        try
        //        {
        //            TResult result = func.Invoke();
        //            return result;
        //        }
        //        catch (Exception ex)
        //        {
        //            return exceptionHandler.Invoke(ex);
        //        }
        //    };
        //}

        //private static bool CanExecuteCommand(IDeviceCommand command, DeviceState deviceState, out Func<IDevice, CommandResult> commandExecute)
        //{
        //    if (command.CanDo(deviceState))
        //    {
        //        commandExecute = command.Do;
        //        return true;
        //    }
        //    else if (command.CanUndo(deviceState))
        //    {
        //        commandExecute = command.Undo;
        //        return true;
        //    }
        //    commandExecute = null;
        //    return false;
        //}
