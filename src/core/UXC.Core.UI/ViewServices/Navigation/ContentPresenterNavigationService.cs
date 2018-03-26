using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.ViewModels;
using UXI.Common.UI;

namespace UXC.Core.ViewServices
{
    public class ContentPresenterNavigationService : BindableBase, INavigationService
    {
        private readonly Stack<object> _backStack = new Stack<object>();

        private object currentContent = null;
        public object CurrentContent
        {
            get { return currentContent; }
            private set { Set(ref currentContent, value); }
        }

        public bool CanGoBack => _backStack.Any();

        private void OnBackStackChanged()
        {
            OnPropertyChanged(nameof(CanGoBack));
        }

        public void Clear()
        {
            CurrentContent = null;
            ClearBackStack();
        }

        public void ClearBackStack()
        {
            _backStack.Clear();
            OnBackStackChanged();
        }

        public void GoBack()
        {
            CurrentContent = _backStack.Pop();
        }

        public void GoHome()
        {
            while (CanGoBack)
            {
                GoBack();
            }
        }

        public bool NavigateToObject(object content)
        {
            if (content is INavigatingViewModel)
            {
                var navigating = (INavigatingViewModel)content;
                navigating.NavigationService = this;
            }

            if (currentContent != content)
            {
                if (currentContent != null)
                {
                    _backStack.Push(currentContent);
                }

                CurrentContent = content;
                return true;
            }

            return false;
        }
    }

}
