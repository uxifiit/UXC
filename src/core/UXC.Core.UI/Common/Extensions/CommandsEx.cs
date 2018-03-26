using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using UXI.Common.Extensions;

namespace UXC.Core.Common.Extensions
{
    public static class Commands
    {
        public static RelayCommand<T> Register<T>(this List<ICommand> commands, Action<T> action, Func<T, bool> canExecute = null)
        {
            commands.ThrowIfNull(nameof(commands));

            var command = new RelayCommand<T>(action, canExecute);
            commands.Add(command);
            return command;
        }

        public static RelayCommand Register(this List<ICommand> commands, Action action, Func<bool> canExecute = null)
        {
            commands.ThrowIfNull(nameof(commands));

            var command = new RelayCommand(action, canExecute);
            commands.Add(command);
            return command;
        }


        public static void TryRaiseCanExecuteChanged(this IEnumerable<ICommand> commands)
        {
            string methodName = nameof(RelayCommand.RaiseCanExecuteChanged);
            var relayCommandType = typeof(RelayCommand);
            var relayCommandOfTType = typeof(RelayCommand<>);

            foreach (var command in commands)
            {
                var type = command.GetType();
                if (type == relayCommandType || type == relayCommandOfTType)
                {
                    type.InvokeMember(methodName, System.Reflection.BindingFlags.InvokeMethod, null, command, null);
                }
            }
        }
    }
}
