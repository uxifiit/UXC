/**
 * UXC.Core.UI
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using UXC.Core.Views;

//namespace UXC.Core.ViewServices
//{
//    public class WindowsService : IWindowsService
//    {
//        public TWindow Open<TWindow>(object dataContext)
//            where TWindow : Window, new()
//        {
//            var window = new TWindow();
//            window.DataContext = dataContext;
//            window.Show();
//            return window;
//        }


//        private DisplayWindow _displayWindow;

//        public Window GetDisplayWindow()
//        {
//            var window = _displayWindow ?? (_displayWindow = new DisplayWindow());

//            return window;
//        }


//        public Window Open(object dataContext)
//        {
//            var window = new Window();
//            window.DataContext = dataContext;
//            window.Show();
//            return window;
//        }
//    }
//}
