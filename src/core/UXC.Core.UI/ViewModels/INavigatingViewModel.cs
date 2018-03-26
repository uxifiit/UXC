using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.ViewServices;

namespace UXC.Core.ViewModels
{
    public interface INavigatingViewModel
    {
        INavigationService NavigationService { get; set; }

        event EventHandler Closed;
    }
}
