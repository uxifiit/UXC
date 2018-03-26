using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;

namespace UXC.Common.Commands
{
    public class ShowInExplorerCommand : RelayCommand<string>
    {
        public ShowInExplorerCommand() : base(path => FileHelper.ShowInExplorer(path))
        {
        }
    }
}
