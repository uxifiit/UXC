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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UXC.Sessions.Common
{
    sealed class RelayJsonConverter<T> : JsonConverter<T>
    {
        private readonly Func<JToken, JsonSerializer, T> _convert;

        public RelayJsonConverter(Func<JToken, JsonSerializer, T> convert)
        {
            _convert = convert;
        }

        protected override T Convert(JToken token, JsonSerializer serializer)
        {
            return _convert.Invoke(token, serializer);  
        }
    }
}
