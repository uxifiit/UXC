using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;

namespace UXC.Common.Commands
{
    public class OpenDirectoryInExplorerCommand : RelayCommand<string>
    {
        public OpenDirectoryInExplorerCommand() : base(path => FileHelper.OpenDirectoryInExplorer(path))
        {
        }
    }
}
