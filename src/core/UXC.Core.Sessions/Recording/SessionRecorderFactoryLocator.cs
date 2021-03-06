/**
 * UXC.Core.Sessions
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
using UXC.Core.Configuration;
using UXC.Core.Managers;
using UXC.Core.Modules;
using UXC.Sessions.Serialization;
using UXI.Common.Extensions;

namespace UXC.Sessions.Recording
{
    public class SessionRecorderFactoryLocator
    {
        private readonly Dictionary<string, ISessionRecorderFactory> _factories = new Dictionary<string, ISessionRecorderFactory>(StringComparer.CurrentCultureIgnoreCase);

        public SessionRecorderFactoryLocator(IEnumerable<ISessionRecorderFactory> factories, IModulesService modules)
        {
            AddFactories(factories);

            modules.Register<ISessionRecorderFactory>(this, AddFactories); 
        }


        private void AddFactories(IEnumerable<ISessionRecorderFactory> factories)
        {
            factories?.Where(factory => String.IsNullOrWhiteSpace(factory.Target) == false)
                      .ForEach(factory => _factories.TryAdd(factory.Target, factory));
        }


        public IEnumerable<ISessionRecorder> CreateForRecording(SessionRecording recording)
        {
            recording.ThrowIfNull(nameof(recording));

            return recording.RecorderConfigurations
                            .Where(r => _factories.ContainsKey(r.Key))
                            .Where(r => (r.Value.ContainsKey("disable") == false) || (r.Value["disable"].Equals(true) == false))
                            .Select(r => 
                            {
                                var recorder = _factories[r.Key].Create(recording);
                                Configurator.Configure(recorder, r.Value);
                                return recorder;
                            })
                   ?? Enumerable.Empty<ISessionRecorder>();
        }
    }
}
