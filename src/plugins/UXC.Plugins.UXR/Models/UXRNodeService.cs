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
using System.Threading;
using System.Threading.Tasks;
using UXC.Plugins.UXR.Extensions;
using UXR.Studies.Client;

namespace UXC.Plugins.UXR.Models
{
    public class UXRNodeService
    {
        private readonly IUXRClient _client;
        private readonly UXRNodeContext _context;
        private readonly UXRStatusUpdateBuilder _statusBuilder;

        internal UXRNodeService(IUXRClient client, UXRNodeContext context, UXRStatusUpdateBuilder statusBuilder)
        {
            _client = client;
            _context = context;
            _statusBuilder = statusBuilder;
        }

        public event EventHandler<IUXRNodeContext> NodeStatusUpdated; 

        public async Task UpdateNodeStatusAsync(CancellationToken cancellationToken)
        {
            try
            {                               // vola sa Cancel, tak dava IsConnected = false
                var update = _statusBuilder.BuildStatusUpdate();
                var nodeInfo = await _client.UpdateNodeStatusAsync(update, cancellationToken);

                if (nodeInfo != null)
                {
                    _context.Update(nodeInfo);

                    NodeStatusUpdated?.Invoke(this, _context);
                }
                else
                {
                    _context.IsConnected = false;
                }
            }
            catch
            {
                _context.IsConnected = false;
            }
        }
    }
}
