
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Common;
using UXC.Core.Common.Events;

namespace UXC.Core
{
    public enum ControlServiceState
    {
        Stopped,
        Running,
        Error
    }

    public interface IControlService : INotifyStateChanged<ControlServiceState>
    {
        bool AutoStart { get; }

        void Start();

        void Stop();

        bool IsWorking();
    }
}
