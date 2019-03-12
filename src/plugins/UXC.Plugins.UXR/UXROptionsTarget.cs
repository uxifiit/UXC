/**
 * UXC.Plugins.UXR
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
using UXC.Core;
using UXC.Plugins.UXR.Models;
using UXI.Common;

namespace UXC.Plugins.UXR
{
    class UXROptionsTarget : IOptionsTarget
    {
        private readonly UXRNodeContext _node;

        public UXROptionsTarget(UXRNodeContext node)
        {
            _node = node;
        }

        public Type OptionsType { get; } = typeof(UXROptions);

        public void ReceiveOptions(object options)
        {
            var uxrOptions = options as UXROptions;
            if (uxrOptions != null && String.IsNullOrWhiteSpace(uxrOptions.NodeName) == false)
            {
                _node.NodeName = uxrOptions.NodeName;
            }                               
        }
    }
}
