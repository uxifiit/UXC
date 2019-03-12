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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UXC.Core.Devices;
using UXI.Common.Converters;

namespace UXC.Common.Converters
{

    public class BoolToTrayIconUriConverter : BoolToValueConverter<Uri>
    {
        public BoolToTrayIconUriConverter()
            : base(new Uri("/Resources/Green.ico", UriKind.RelativeOrAbsolute), new Uri("/Resources/Red.ico", UriKind.RelativeOrAbsolute))
        { }
    }
}
