using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.ViewServices
{
    public interface ITrayIconProvider
    {
        TaskbarIcon Icon { get; }
        bool CanShowNotifications { get; }
    }
}
