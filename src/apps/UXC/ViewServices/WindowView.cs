using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UXI.Common.Extensions;
using UXI.Common.Helpers;
using UXC.Core.ViewServices;
using UXC.Core.Common.Extensions;

namespace UXC.ViewServices
{
    public class WindowView : IView
    {
        private readonly Window _window;

        public WindowView(Window window)
        {
            _window = window;
            _window.Closed += window_Closed;
        }


        private void window_Closed(object sender, EventArgs e)
        {
            IsClosed = true;
            Closed?.Invoke(this, e);
        }


        private INavigationService content = null;
        public INavigationService Navigation => content ?? (content = new ContentFrameNavigationService(_window));


        public bool IsActive => _window.IsActive;


        public bool IsClosed { get; private set; } = true;


        public void Show(string header)
        {
            // TODO header
            Show();
        }


        public Task ShowAsync(string header)
        {
            // TODO header
            return ShowAsync();
        }


        public void Activate()
        {
            _window.Activate();
            _window.Topmost = true;
            _window.Topmost = false;
            _window.Focus();
        }


        public void MakeVisible()
        {
            //            _window.ShowInTaskbar = true;
            _window.Topmost = true;
            _window.Visibility = Visibility.Visible;
            _window.Topmost = false;
        }


        public void Hide()
        {
            _window.Hide();
//            _window.ShowInTaskbar = false;
//            _window.Visibility = Visibility.Collapsed;
        }


        public void Show()
        {
            _window.Topmost = true;
            _window.Show();
            _window.Topmost = false;

            IsClosed = false;
        }


        public Task ShowAsync()
        {
            return AsyncHelper.InvokeAsync<EventHandler>(
                Show,
                h => _window.Closed += h,
                h => _window.Closed -= h,
                tcs => (s, e) => tcs.TrySetResult(true));
        }


        public void Close()
        {
            _window.Close();
        }

        public event EventHandler Closed;
    }
}
