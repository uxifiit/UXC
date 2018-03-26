using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;

namespace UXC.Common.Commands
{
    public class ShutdownAppCommand : RelayCommand
    {
        public ShutdownAppCommand()
            : base(() => Application.Current.Shutdown())
        { }
    }
}
