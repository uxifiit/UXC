/**
 * UXC.Core.UI
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UXI.Common.Helpers;
using System.Threading;

namespace UXC.Core.ViewServices
{
    public class NotificationService : INotificationService
    {
        private readonly ITrayIconProvider _icon;

        public NotificationService(ITrayIconProvider icon)
        {
            _icon = icon;
        }

        //private bool CanShowNotification
        //{
        //    get { return (App.Current.MainWindow == null || App.Current.MainWindow.IsActive == false); }
        //}
        public void ShowInfoMessage(string title, string message)
        {
            if (_icon.CanShowNotifications)
            {
                _icon.Icon?.ShowBalloonTip(title, message, BalloonIcon.Info);
            }
        }

        public void ShowErrorMessage(string title, string message)
        {
            if (_icon.CanShowNotifications)
            {
                _icon.Icon?.ShowBalloonTip(title, message, BalloonIcon.Error);
            }
        }

        public async Task<bool> ShowRequestMessageAsync(string title, string message, CancellationToken cancellationToken)
        {
            if (_icon.CanShowNotifications)
            {
                var icon = _icon.Icon;
                if (icon != null)
                {
                    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();  // TODO ? TaskCreationOptions.AttachedToParent

                    cancellationToken.Register(() => tcs.TrySetCanceled());

                    RoutedEventHandler clickedHandler = (sender, args) => tcs.TrySetResult(true);
                    RoutedEventHandler closedHandler = (sender, args) => tcs.TrySetResult(false);

                    icon.TrayBalloonTipClicked += clickedHandler;
                    icon.TrayBalloonTipClosed += closedHandler;

                    try
                    {
                        icon.ShowBalloonTip(title, message, BalloonIcon.Info);

                        return await tcs.Task;
                    }
                    finally
                    {
                        icon.TrayBalloonTipClicked -= clickedHandler;
                        icon.TrayBalloonTipClosed -= closedHandler;
                    }
                }
            }

            return false;
        }


        //public void ShowAdaptersNotification(AdaptersNotificationViewModel notification)
        //{
        //    var icon = GetIcon();
        //    if (icon != null)
        //    {
        //        icon.ShowCustomBalloon(new AdaptersNotification() { DataContext = notification, Width = 300, Height = 300 }, System.Windows.Controls.Primitives.PopupAnimation.Fade, 5000);
        //    }
        //}
    }
}
