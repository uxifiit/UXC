using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.ViewServices
{
    public interface INavigationService
    {
        bool NavigateToObject(object content);
        void GoHome();
        void GoBack();
        bool CanGoBack { get; }

        void ClearBackStack();
        void Clear();

    }
}
