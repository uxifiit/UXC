using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UXC.Core.Common.Commands
{
    public class NullCommand : ICommand
    {
        public static readonly NullCommand Instance = new NullCommand();

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public bool CanExecute(object parameter) => false;

        public void Execute(object parameter) { }
    }
}
