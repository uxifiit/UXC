/**
 * UXC.Core.Sessions.UI
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System.Windows.Media.Imaging;

namespace UXC.Sessions.Models
{
    public interface IImageService
    {
        void Add(string path);
        void Clear();
        bool Contains(string path);
        bool TryGet(string path, out BitmapSource image);
    }
}