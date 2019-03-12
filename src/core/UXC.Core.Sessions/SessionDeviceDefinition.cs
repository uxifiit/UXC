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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;

namespace UXC.Sessions
{
    public class SessionDeviceDefinition
    {
        private static readonly IReadOnlyDictionary<string, object> EmptyConfiguration = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());

        public SessionDeviceDefinition(DeviceType device, IReadOnlyDictionary<string, object> configuration = null)
        {
            Device = device;
            Configuration = configuration != null && configuration.Any()
                            ? new ReadOnlyDictionary<string, object>(configuration?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
                            : EmptyConfiguration;
        }

        public DeviceType Device { get; }
        public IReadOnlyDictionary<string, object> Configuration { get; }

        public SessionDeviceDefinition Clone()
        {
            return new SessionDeviceDefinition(Device, Configuration);
        }


        public static readonly IEqualityComparer<SessionDeviceDefinition> ComparerByKey = new EqualityComparerByKey();

        private class EqualityComparerByKey : IEqualityComparer<SessionDeviceDefinition>
        {
            public bool Equals(SessionDeviceDefinition x, SessionDeviceDefinition y)
            {
                return (x == null && y == null)
                    || (x != null && y != null && x.Device.Equals(y.Device));
            }

            public int GetHashCode(SessionDeviceDefinition obj)
            {
                return obj.Device.GetHashCode();
            }
        }
    }


    public class SessionRecorderDefinition
    {
        private static readonly IReadOnlyDictionary<string, object> EmptyConfiguration = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());

        public SessionRecorderDefinition(string name, IReadOnlyDictionary<string, object> configuration = null)
        {
            Name = name;
            Configuration = configuration != null && configuration.Any()
                            ? new ReadOnlyDictionary<string, object>(configuration?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
                            : EmptyConfiguration;
        }

        public string Name { get; }
        public IReadOnlyDictionary<string, object> Configuration { get; }

        public SessionRecorderDefinition Clone()
        {
            return new SessionRecorderDefinition(Name, Configuration);
        }


        public static readonly IEqualityComparer<SessionRecorderDefinition> ComparerByKey = new EqualityComparerByKey();

        private class EqualityComparerByKey : IEqualityComparer<SessionRecorderDefinition>
        {
            public bool Equals(SessionRecorderDefinition x, SessionRecorderDefinition y)
            {
                return (x == null && y == null)
                    || (x != null && y != null
                        && ((String.IsNullOrWhiteSpace(x.Name) && String.IsNullOrWhiteSpace(y.Name))
                        || x.Name.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase)));
            }

            public int GetHashCode(SessionRecorderDefinition obj)
            {
                return obj.Name.GetHashCode();
            }
        }
    }
}
