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
using UXI.Common.Helpers;

namespace UXC.Core.ViewServices
{
    public class InteractionService : IInteractionService
    {
        public bool RequestInteraction(object source, InteractionRequest request)
        {
            var args = new InteractionRequestEventArgs(source, request);

            InteractionRequested?.Invoke(source, args);

            return args.IsAccepted;
        }


        public event EventHandler<InteractionRequestEventArgs> InteractionRequested;
    }
}
