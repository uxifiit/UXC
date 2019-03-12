/**
 * UXC.Core.Interfaces
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;

namespace UXC.Core.Configuration
{
    public class Configurator : IConfigurator
    {
        private readonly IConfigurable _target;

        public Configurator(IConfigurable target)
        {
            target.ThrowIfNull(nameof(target));

            _target = target;
        }


        public bool CanConfigure() => CanConfigure(_target);

        public static bool CanConfigure(IConfigurable target) => target.Configuration?.Settings != null && target.Configuration.Settings.Any();


        public void Configure(IDictionary<string, object> values)
        {
            Configure(_target, values);
        }

        public static void Configure(IConfigurable target, IDictionary<string, object> values)
        {
            var settings = target.Configuration.Settings.ToDictionary(s => s.Key);

            values.Where(setting => settings.ContainsKey(setting.Key))
                  .ForEach(setting => settings[setting.Key].SetValue(setting.Value));
        }


        public void Reset(string key)
        {
            Reset(_target, key);
        }

        public static void Reset(IConfigurable target, string key)
        {
            target.Configuration.Settings.FirstOrDefault(s => s.Key == key)?.Reset();
        }


        public void ResetAll()
        {
            ResetAll(_target);
        }

        public static void ResetAll(IConfigurable target)
        {
            target.Configuration.Settings.ForEach(s => s.Reset());
        }
    }
}
