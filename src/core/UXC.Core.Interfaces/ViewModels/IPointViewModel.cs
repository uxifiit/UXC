/**
 * UXC.Core.Interfaces
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System.ComponentModel;

namespace UXC.Core.ViewModels
{
    public interface IPointViewModel : INotifyPropertyChanged
    {
        bool IsVisible { get; }
        double X { get; }
        double Y { get; }
    }
}
