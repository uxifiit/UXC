using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardcodet.Wpf.TaskbarNotification;
using UXC.Core.ViewServices;

namespace UXC.ViewServices
{
    public class TrayIconProvider : ITrayIconProvider
    {

        public bool CanShowNotifications
        {
            get
            {
                return (App.Current.MainWindow == null || App.Current.MainWindow.IsActive == false);
            }
            set
            {
            }
        }

        public TaskbarIcon Icon
        {
            get; set;
        }
    }
}
