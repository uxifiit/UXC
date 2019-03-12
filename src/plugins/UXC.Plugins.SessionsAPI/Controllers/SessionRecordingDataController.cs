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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using UXC.Plugins.SessionsAPI.Services;
using UXI.Common.Web;

namespace UXC.Plugins.SessionsAPI.Controllers
{
    [RoutePrefix(ApiRoutes.Recording.Data.PREFIX)]
    public class SessionRecordingDataController  : ApiController
    {
        private readonly SessionRecordingResultsService _service;

        public SessionRecordingDataController(SessionRecordingResultsService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route(ApiRoutes.Recording.Data.ACTION_DOWNLOAD_GAZE)]
        public IHttpActionResult DownloadEyeGazeData(string recording)
        {
            string filepath;
            string filename;

            if (String.IsNullOrWhiteSpace(recording) == false 
                && (recording.Equals(ApiRoutes.Recording.PARAM_RECENT_RECORDING, StringComparison.InvariantCultureIgnoreCase)
                    && _service.TryGetEyeGazeDataFile(out filepath, out filename))
                || _service.TryGetEyeGazeDataFile(recording, out filepath, out filename))
            {
                return new FileActionResult(filepath, filename, "application/json");
            }
            return NotFound();
        }


        [HttpGet]
        [Route(ApiRoutes.Recording.Data.ACTION_DOWNLOAD_FIXATIONS)]
        public IHttpActionResult DownloadEyeFixations(string recording)
        {
            string filepath;
            string filename;

            if (String.IsNullOrWhiteSpace(recording) == false
                && (recording.Equals(ApiRoutes.Recording.PARAM_RECENT_RECORDING, StringComparison.InvariantCultureIgnoreCase)
                    && _service.TryGetEyeFixationsFile(out filepath, out filename))
                || _service.TryGetEyeFixationsFile(recording, out filepath, out filename))
            {
                return new FileActionResult(filepath, filename, "application/json");
            }
            return NotFound();
        }
    }
}
