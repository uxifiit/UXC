using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UXC.Core.ViewServices;
using UXC.Core.Common.Extensions;
using UXC.Core.ViewModels;
using UXI.Common.UI;

namespace UXC.Core.ViewServices
{
   
    public class ContentFrameNavigationService : INavigationService
    {
        private readonly string _frameName;
        private readonly DependencyObjectProvider _host;
        public ContentFrameNavigationService(DependencyObjectProvider host, string frameName)
        {
            _host = host;
            _frameName = frameName;
        }
        public ContentFrameNavigationService(DependencyObject host)
        {
            _host = new DependencyObjectProvider(() => host);
            _frameName = null;
        }

        

        private Frame frame = null;
        protected Frame ContentFrame => frame ?? (frame = GetContentFrame());

        protected virtual Frame GetContentFrame()
        {
            var frames = _host.Invoke().GetChildrenRecursive().OfType<Frame>();
            if (String.IsNullOrWhiteSpace(_frameName) == false)
            {
                frames = frames.Where(f => f.Name == _frameName);
            }
            return frames.FirstOrDefault();
        }

        #region INavigationService Members

        //public bool Navigate(Uri page)
        //{
        //    return ContentFrame.NavigationService.Navigate(page);
        //}

        //public bool Navigate(Uri page, object state)
        //{
        //    return ContentFrame.NavigationService.Navigate(page, state);
        //}

        public bool NavigateToObject(object content)
        {
            if (content is INavigatingViewModel)
            {
                var navigating = (INavigatingViewModel)content;
                navigating.NavigationService = this;
            }

            if (ContentFrame.Content != content)
            {
                return ContentFrame.NavigationService.Navigate(content);
            }

            return false;
        }

        public void GoBack()
        {
            ContentFrame.NavigationService.GoBack();
        }

        //public void GoBackOrNavigate(Uri page)
        //{
        //    var frame = ContentFrame;

        //    List<JournalEntry> entries = frame.BackStack?.OfType<JournalEntry>().ToList();
        //    if (entries?.Any() == true)
        //    {
        //        var search = entries.FirstOrDefault(p => p.Source.OriginalString.Trim('/').Equals(page.OriginalString.Trim('/')));
        //        if (search != null)
        //        {
        //            int index = entries.IndexOf(search) - entries.Count + 1;
        //            while (index++ < 0 && frame.NavigationService.CanGoBack)
        //            {
        //                frame.NavigationService.GoBack();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        frame.NavigationService.Navigate(page);
        //    }
        //}

        public void GoHome()
        {
            var navigation = ContentFrame.NavigationService;
            
            while (navigation.CanGoBack)
            {
                navigation.GoBack();
            }
        }

        public bool CanGoBack
        {
            get { return ContentFrame.NavigationService.CanGoBack; }
        }

        public void ClearBackStack()
        {
            var navigation = ContentFrame.NavigationService;
            while (navigation.CanGoBack)
            {
                navigation.RemoveBackEntry();
            }
        }

        public void Clear()
        {
            ContentFrame.Content = null;
            ClearBackStack();
        }

    //    public Uri CurrentSource { get { return ContentFrame.NavigationService.CurrentSource; } }

        #endregion
    }
}
