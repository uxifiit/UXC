using System;
using UXC.Core.Common.Events;

namespace UXC.Core.ViewServices
{
    public interface IAppService : INotifyStateChanged<AppState>
    {
        event EventHandler<Exception> Error;

        bool Load();
        bool Start();
        bool Stop();

        bool CheckIfStopCancelsWorkInProgress();
    }
}
