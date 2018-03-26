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
    public class FlyoutView : IDialogView
    {
        private readonly string _name;
        private readonly DependencyObject _host;

        public FlyoutView(DependencyObject host, string flyoutName)
        {
            _host = host;
            _name = flyoutName;
        }

        private Flyout flyout = null;
        protected Flyout Flyout
        {
            get
            {
                if (flyout != null)
                {
                    return flyout;
                }
                flyout = _host.GetChildrenRecursive().OfType<Flyout>().FirstOrDefault(f => f.Name == _name);

                flyout.ThrowIfNull(() => new InvalidOperationException($"No flyout with name {_name} was found."));

                return flyout;
            }
        }

        private INavigationService content = null;
        public INavigationService Navigation => content ?? (content = new ContentFrameNavigationService(Flyout));

        public DialogPosition Position { get; set; }

        public bool IsClosed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsActive
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Show(string header) //, DialogPosition position)
        {
            var flyout = Flyout;
            flyout.Header = header.ToLower();
            Show();
        }

        public Task ShowAsync(string header)//, DialogPosition position)
        {
            var flyout = Flyout;

            flyout.Header = header.ToLower();
            return ShowAsync();
        }

        public void Show()
        {
            var flyout = Flyout;
            flyout.Position = (Position)Position;
            Flyout.IsOpen = true;
        }

        public Task ShowAsync()
        {
            var flyout = Flyout;
            flyout.Position = (Position)Position;
            return AsyncHelper.InvokeAsync<RoutedEventHandler>(
                Show,
                h => flyout.ClosingFinished += h,
                h => flyout.ClosingFinished -= h,
                tcs => (s, e) => tcs.TrySetResult(true));
        }


        public void Close()
        {
            Flyout.IsOpen = false;
        }

        public void Activate()
        {
        }

        public void Hide()
        {
        }

        public void MakeVisible() { }


        // TODO
        public event EventHandler Closed;
    }
}
