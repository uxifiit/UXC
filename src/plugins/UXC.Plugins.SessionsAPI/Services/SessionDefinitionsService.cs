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
using UXC.Plugins.SessionsAPI.Entities;
using UXC.Sessions;
using UXI.Common.Extensions;

namespace UXC.Plugins.SessionsAPI.Services
{
    public class SessionDefinitionsService
    {
        private readonly ExternalSessionDefinitions _definitions;
        private readonly IMapper _mapper;

        public SessionDefinitionsService(ExternalSessionDefinitions definitions, IMapper mapper)
        {
            _definitions = definitions;
            _mapper = mapper;
        }


        public List<SessionDefinitionInfo> GetList()
        {
            var definitions = _definitions.Definitions
                                          .Select(_mapper.Map<SessionDefinitionInfo>)
                                          .ToList();
            return definitions;
        }


        public SessionDefinitionInfo Create(SessionDefinitionCreate session)
        {
            session.ThrowIfNull(nameof(session));

            var definition = _mapper.Map<SessionDefinition>(session);

            _definitions.Add(definition);

            return _mapper.Map<SessionDefinitionInfo>(definition);
        }


        public SessionDefinitionInfo GetDetails(int definitionId)
        {
            SessionDefinition definition;
            if (_definitions.Definitions.TryGet(d => d.Id == definitionId, out definition))
            {
                return _mapper.Map<SessionDefinitionInfo>(definition);
            }

            return null;
        }
    }
}
