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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UXC.Sessions
{
    class LocalSessionDefinitionsService : ILocalSessionDefinitionsService
    {
        private readonly JsonSerializer _serializer;
        public LocalSessionDefinitionsService(IEnumerable<JsonConverter> converters)
        {
            _serializer = new JsonSerializer();
            converters.ToList().ForEach(_serializer.Converters.Add);
        }


        public SessionDefinition LoadFromFile(string filepath)
        {
            using (var reader = new StreamReader(filepath))
            {
                // deserialize
                var definition = SessionDefinition.Create();
                _serializer.Populate(reader, definition);

                //definition.CreatedAt = file.LastWriteTime;

                return definition;
            }
        }
    }
}
