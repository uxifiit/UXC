/**
 * UXC
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UXC.Core.ViewServices;
using UXC.Views;
using UXI.Common.Extensions;

namespace UXC.ViewServices
{
    public class ViewsService : IViewsService
    {
        private readonly DependencyObjectProvider _mainWindowProvider;
        public ViewsService(DependencyObjectProvider mainWindowProvider)
        {
            _mainWindowProvider = mainWindowProvider;
        }


        private IView mainWindow;
        public IView MainWindow
        {
            get
            {
                if (mainWindow == null)
                {
                    var window = _mainWindowProvider.Invoke() as Window;
                    if (window != null)
                    {
                        mainWindow = new WindowView(window);
                    }
                }
                return mainWindow;
            }
        }


        private IView sessionHostWindow;
        public IView SessionHostWindow
        {
            get
            {
                var window = sessionHostWindow;
                if (window == null || window.IsClosed)
                {
                    sessionHostWindow = new WindowView(new HostWindow());
                }

                return sessionHostWindow;
            }
        }


        private IView settings = null;
        public IView Settings
        {
            get
            {
                return settings ?? (settings = new FlyoutView(_mainWindowProvider.Invoke(), "SettingsFlyout"));
            }
        }
    }
}
