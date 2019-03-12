/**
 * UXC.Core.UI
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

namespace UXC.Core.ViewServices
{
    public enum DialogPosition
    {
        Left = 0,
        Right = 1,
        Top = 2,
        Bottom = 3
   //     Overlay = 4
    }

    public interface IViewsService
    {
        IView MainWindow { get; }
        IView SessionHostWindow { get; }
        IView Settings { get; }
    }


    public interface IDialogView : IView
    {
        DialogPosition Position { get; set; }
    }


    public interface IView
    {
        void Show(string header);
        Task ShowAsync(string header);
        void Show();
        Task ShowAsync();
        void Close();
        void Activate();
        void MakeVisible();
        void Hide();
        bool IsClosed { get; } 
        bool IsActive { get; }

        event EventHandler Closed;

        INavigationService Navigation { get; }
    }
}
