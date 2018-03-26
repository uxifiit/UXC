using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;

namespace UXC.Sessions.ViewModels
{
    public interface ISessionChoiceViewModel
    {
        int Id { get; }
        string Name { get; set; }

        // TODO use or remove, not used in the Views
        bool CanEditName { get; }

        ICollection<DeviceType> SelectedDevices { get; }

        string ChoiceName { get; }

        // TODO use or remove, not used in the Views
        bool CanEditDevices { get; }

        //bool CanLockDevices { get; } 

        SessionDefinition GetDefinition();
    }
}
