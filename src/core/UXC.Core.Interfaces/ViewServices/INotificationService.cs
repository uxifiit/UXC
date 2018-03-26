using System;
using System.Threading;
using System.Threading.Tasks;

namespace UXC.Core.ViewServices
{
    public interface INotificationService
    {
        void ShowErrorMessage(string title, string message);
        void ShowInfoMessage(string title, string message);
        Task<bool> ShowRequestMessageAsync(string title, string message, CancellationToken cancellationToken);
    }
}
