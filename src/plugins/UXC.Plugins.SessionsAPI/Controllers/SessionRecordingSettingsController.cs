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
    [RoutePrefix(ApiRoutes.Recording.Settings.PREFIX)]
    public class SessionRecordingSettingsController : ApiController
    {
        private readonly SessionRecordingSettingsService _service;

        public SessionRecordingSettingsController(SessionRecordingSettingsService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route(ApiRoutes.Recording.Settings.ACTION_KEY)]
        [ResponseType(typeof(object))]
        public IHttpActionResult Get(string section, string key)
        {
            try
            {
                return Ok(_service.GetSetting(section, key));
            }
            catch 
            {
                return BadRequest("Parameters cannot be null.");
            }
        }


        [HttpPost]
        [Route(ApiRoutes.Recording.Settings.ACTION_KEY)]
        public IHttpActionResult Set(string section, string key, [FromBody]string value)
        {
            try
            {
                //string value = await Request.Content.ReadAsStringAsync(); // hack instead of implementing text/plain formatter
                if (_service.TrySetSetting(section, key, value))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("No session recording running.");
                } 
            }
            catch
            {
                return BadRequest("Parameters cannot be null.");
            }
        }
    }
}
