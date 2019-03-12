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
using UXC.Core.Data;
using UXC.Sessions.Serialization.Converters.Json;
using UXI.Common.Extensions;

namespace UXC.Sessions.Timeline.Actions
{
    public abstract class SessionStepActionSettings
    {
        private readonly static Dictionary<string, Type> Types = new Dictionary<string, Type>();
        private readonly static Type BaseType = typeof(SessionStepActionSettings);


        private static string CreateActionTypeName(Type type) => type.Name.Replace("ActionSettings", "");


        public static void Register(Type type)
        {
            Register(type, CreateActionTypeName(type));
        }


        public static void Register(Type type, string name)
        {
            type.ThrowIfNull(nameof(type))
                .ThrowIf(t => BaseType.IsAssignableFrom(t) == false, nameof(type), $"Instances of the type {type.FullName} cannot be assigned to the type {BaseType.FullName}.");

            Types.TryAdd(name.ToLower(), type);
        }
 

        public static bool TryResolve(string name, out Type type)
        {
            if (String.IsNullOrWhiteSpace(name) == false)
            {
                return Types.TryGetValue(name.Trim().ToLower(), out type);
            }

            type = null;
            return false;
        }


        private string actionType = null;
        public virtual string ActionType => actionType 
            ?? (actionType = CreateActionTypeName(this.GetType()));   // Used for serialization? 


        public string Tag { get; set; }


        public virtual SessionStepActionSettings Clone()
        {
            return (SessionStepActionSettings)this.MemberwiseClone();
        }
    }
}
