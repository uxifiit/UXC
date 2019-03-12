/**
 * UXC.Plugins.SessionsAPI
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
using AutoMapper;
using Microsoft.AspNet.SignalR;
using UXC.Plugins.SessionsAPI.Entities;
using UXC.Plugins.SessionsAPI.Services;
using UXC.Sessions;

namespace UXC.Plugins.SessionsAPI.Hubs
{
    public class SessionDefinitionHub : Hub
    {
        private readonly SessionDefinitionsService _service;

        public SessionDefinitionHub(SessionDefinitionsService service)
        {
            _service = service;
        }


        public List<SessionDefinitionInfo> GetList()
        {
            return _service.GetList();
        }


        public SessionDefinitionInfo Create(SessionDefinitionCreate session)
        {
            return _service.Create(session);
        }


        public SessionDefinitionInfo GetDetails(int definitionId)
        {
            return _service.GetDetails(definitionId);
        }
    }
}
