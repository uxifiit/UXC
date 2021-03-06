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
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UXC.Configuration;
using UXC.Core;
using UXC.Core.Modules;
using UXC.Common.Extensions;
using UXC.Core.Common.Events;
using UXI.OwinServer;

namespace UXC.Services
{
    internal class CustomContentTypeProvider : Microsoft.Owin.StaticFiles.ContentTypes.FileExtensionContentTypeProvider
    {
        public CustomContentTypeProvider()
        {
            Mappings.Add(".md", "text/markdown");
        }
    }


    public class ServerControlService : NotifyStateChangedBase<ControlServiceState>, IControlService
    {
        private readonly IServerConfiguration _configuration;
        private readonly ServerHost _server;


        public ServerControlService
        (
            ServerHost server,
            IServerConfiguration configuration,
            IEnumerable<JsonConverter> converters,
            IEnumerable<MediaTypeFormatter> formatters,
            IModulesService modules
        )
        {
            _server = server;
            _configuration = configuration;

            AddConverters(converters);
            AddFormatters(formatters);

            modules.Register<JsonConverter>(this, AddConverters);
            modules.Register<MediaTypeFormatter>(this, AddFormatters);

            _server.CustomFileExtensionContentTypeProvider = new CustomContentTypeProvider();
        }

        private void AddFormatters(IEnumerable<MediaTypeFormatter> formatters)
        {
            if (formatters != null)
            {
                _server.Formatters.TryAddRange(formatters);
            }
        }

        private void AddConverters(IEnumerable<JsonConverter> converters)
        {
            if (converters != null)
            {
                _server.Converters.TryAddRange(converters);
            }
        }


        public bool AutoStart => true;


        public bool HasFailed { get; private set; }


        public void Start()
        {
            _server.UseSignalR = _configuration.SignalREnabled;
            _server.UseFileServer = false;

            if (_configuration.ServerPort > 0)
            {
                try
                {
                    string serverEndpoint = $"{(_configuration.SslEnabled ? "https" : "http")}://+:{_configuration.ServerPort}/";
                    _server.Start(serverEndpoint);
                    HasFailed = false;
                    State = ControlServiceState.Running;
                }
                catch (Exception ex)
                {
                    _server.Stop();

                    HasFailed = true;
                    Failed?.Invoke(this, EventArgs.Empty);
                    State = ControlServiceState.Error;
                }
            }
        }


        public void Stop()
        {
            _server.Stop();
            State = ControlServiceState.Stopped;
        }


        public event EventHandler Failed;


        public bool IsWorking() => false;
    }
}