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
using UXC.Sessions.Common;
using UXC.Sessions.Timeline.Actions;
using UXI.Serialization.Formats.Json.Extensions;
using UXI.Serialization.Formats.Json.Converters;

namespace UXC.Sessions.Serialization.Converters.Json
{
    public class TextJsonConverter : GenericJsonConverter<Text>
    {
        protected override Text Convert(JToken token, JsonSerializer serializer)
        {
            List<string> lines = null;

            if (token.Type == JTokenType.String)
            {
                lines = new List<string>() { token.Value<string>() };
            }
            else if (token.Type == JTokenType.Array)
            {
                var array = (JArray)token;
                lines = array.Select(e => e.Value<string>()).ToList();
            }
            else if (token.Type == JTokenType.Object) 
            {
                var obj = (JObject)token;
                lines = obj.GetValue<List<string>>(nameof(Text.Lines), serializer);
            }

            return new Text() { Lines = lines };
        }


        public override bool CanWrite => true;


        protected override JToken ConvertBack(Text value, JsonSerializer serializer)
        {
            Text text = value as Text;

            if (text != null)
            {
                JArray array = new JArray();

                foreach (var line in text.Lines)
                {
                    array.Add(line);
                }

                return array;
            }
            
            return JValue.CreateNull();
        }
    }
}
