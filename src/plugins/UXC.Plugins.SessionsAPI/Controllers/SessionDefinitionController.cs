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
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using UXC.Core.Devices;
using UXC.Plugins.SessionsAPI.Entities;
using UXC.Plugins.SessionsAPI.Services;
using UXC.Sessions;


namespace UXC.Plugins.SessionsAPI.Controllers
{
    [RoutePrefix(ApiRoutes.Definition.PREFIX)]
    public class SessionDefinitionController : ApiController
    {
        private readonly SessionDefinitionsService _service;

        public SessionDefinitionController(SessionDefinitionsService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route(ApiRoutes.Definition.ACTION_LIST)]
        [ResponseType(typeof(List<SessionDefinitionInfo>))]
        public IHttpActionResult GetList()
        {
            return Ok(_service.GetList());
        }


        [HttpPost]
        [Route(ApiRoutes.Definition.ACTION_CREATE)]
        [ResponseType(typeof(SessionDefinitionInfo))]
        public IHttpActionResult Create([FromBody] SessionDefinitionCreate session)
        {
            try
            {
                return Ok(_service.Create(session));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        [HttpGet]
        [Route(ApiRoutes.Definition.ACTION_DETAILS)]
        [ResponseType(typeof(SessionDefinitionInfo))]
        public IHttpActionResult GetDetails(int definitionId)
        {
            SessionDefinitionInfo definition = _service.GetDetails(definitionId);
            if (definition != null)
            {
                return Ok(definition);
            }

            return NotFound();
        }
    }
}
