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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common;
using UXC.Core.Devices;
using UXC.Sessions;
using UXR.Studies.Api.Entities.Sessions;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline;
using System.Threading;
using UXR.Studies.Client;
using Newtonsoft.Json;

namespace UXC.Plugins.UXR.Models
{
    public class UXRSessionDefinitionsSource : ISessionDefinitionsSource
    {
        private readonly IUXRClient _client;
        private readonly ConcurrentDictionary<int, SessionDefinition> _definitions = new ConcurrentDictionary<int, SessionDefinition>();


        private readonly JsonSerializerSettings _serializerSettings;

        public UXRSessionDefinitionsSource(IUXRClient client, IEnumerable<JsonConverter> converters)
        {
            _client = client;

            _serializerSettings = new JsonSerializerSettings();
            converters.ToList().ForEach(_serializerSettings.Converters.Add);
        }


        public IEnumerable<SessionDefinition> Definitions => _definitions.Values.OrderBy(d => d.CreatedAt);


        public bool ContainsUXRDefinition(int uxrSessionDefinitionId) => _definitions.ContainsKey(uxrSessionDefinitionId);


        private Dictionary<int, SessionDefinition> CreateDefinitions(IEnumerable<SessionInfo> sessions)
        {
            //var newUXRSessions = sessions.Where(i => ContainsUXRDefinition(i.Id) == false).ToList();
            var definitions = new Dictionary<int, SessionDefinition>();

            foreach (var uxrSession in sessions)
            {
                SessionDefinition definition = null;
                if (TryCreateSessionDefinition(uxrSession, out definition))
                {
                    definitions.Add(uxrSession.Id, definition);
                }
                else
                {
                    // TODO LOG failed adding UXR definition
                }
            }

            return definitions;
        }


        private void UpdateDefinitions(Dictionary<int, SessionDefinition> definitions)
        {
            var newDefinitions = definitions.Where(kvp => _definitions.ContainsKey(kvp.Key) == false
                                                       || _definitions[kvp.Key].CreatedAt != kvp.Value.CreatedAt)
                                            .ToList();
            if (newDefinitions.Any())
            {
                List<SessionDefinition> addedDefinitions = new List<SessionDefinition>();
                List<SessionDefinition> removedDefinitions = newDefinitions.Where(d => _definitions.ContainsKey(d.Key))
                                                                           .Select(d => _definitions[d.Key]).ToList();

                foreach (var kvp in newDefinitions)
                {
                    if (_definitions.AddOrUpdate(kvp.Key, kvp.Value, (id, value) => value.CreatedAt < kvp.Value.CreatedAt ? kvp.Value : value) == kvp.Value)
                    {
                        addedDefinitions.Add(kvp.Value);
                    }
                }

                DefinitionsChanged?.Invoke(this, CollectionChangedEventArgs<SessionDefinition>.Create(addedDefinitions.OrderBy(d => d.CreatedAt), removedDefinitions));
            }
        }


        private bool TryCreateSessionDefinition(SessionInfo sessionInfo, out SessionDefinition definition)
        {
            try
            {
                definition = SessionDefinition.Create();
                string definitionJson = (string)sessionInfo.Definition.Value ?? String.Empty;

                JsonConvert.PopulateObject(definitionJson, definition, _serializerSettings);

                definition.Project = sessionInfo.Project;
                definition.Name = sessionInfo.Name;
                definition.CreatedAt = sessionInfo.CreatedAt;
                definition.Source = "UXR";
                definition.SerializationFormat = "JSON";

                if (definition.Recorders.Any(r => r.Name.Equals("Local", StringComparison.CurrentCultureIgnoreCase)) == false)
                {
                    definition.Recorders.Add(new SessionRecorderDefinition("Local"));
                }

                if (definition.Recorders.Any(r => r.Name.Equals("UXR", StringComparison.CurrentCultureIgnoreCase)) == false)
                {
                    definition.Recorders.Add(new SessionRecorderDefinition("UXR"));
                }

                return true;
            }
            catch (Exception ex)
            {
                
            }

            definition = null;
            return false;
        }


        public bool TryGetUXRSessionId(SessionDefinition definition, out int id)
        {
            int? sessionId = _definitions.Where(kvp => kvp.Value.Id == definition.Id)
                                         .Select(kvp => kvp.Key)
                                         .FirstOrDefault();
            id = sessionId ?? -1;

            return sessionId.HasValue;
        }


        public async Task RefreshAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            IEnumerable<SessionInfo> sessions = await _client.GetCurrentSessionsAsync();

            cancellationToken.ThrowIfCancellationRequested();

            if (sessions != null && sessions.Any())
            {
                Dictionary<int, SessionDefinition> definitions = CreateDefinitions(sessions);

                UpdateDefinitions(definitions);
            }
            else
            {
                // TODO LOG
            }
        }


        public event EventHandler<CollectionChangedEventArgs<SessionDefinition>> DefinitionsChanged;
    }
}
