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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UXC.Sessions.Common
{
    public abstract class InheritedObjectJsonConverter<T> : JsonConverter
    {
        private readonly bool _canBeNull = (typeof(T).IsValueType == false) || (Nullable.GetUnderlyingType(typeof(T)) != null);

        protected abstract Type ResolveType(JObject jObject);

        protected T Create(Type objectType, JObject jObject)
        {
            Type type = ResolveType(jObject);

            if (type == null)
            {
                throw new Exception(String.Format("The given type is not supported!"));
            }
            else
            {
                return (T)Activator.CreateInstance(type);
            }
        }


        public override bool CanConvert(Type objectType)
        {
            return typeof(T).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null && _canBeNull)
            {
                return null;
            }
            else
            {
                // Load JObject from stream
                JObject jObject = JObject.Load(reader);

                // Create target object based on JObject
                T target;
                try
                {
                    target = Create(objectType, jObject);
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Failed to deserialize JSON");
                    throw; 
                }

                // Populate the object properties
                serializer.Populate(jObject.CreateReader(), target);

                return target;
            }
        }

        public override bool CanRead => true;
    }
}
