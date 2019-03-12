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

namespace UXC.Sessions.Common.Helpers
{
    public static class SessionRecordingSettingsHelper
    {
        public static string FillParameters(string text, List<string> parameters, SessionRecordingSettings settings)
        {
            string result = text;

            object recordingSetting;

            foreach (var parameter in parameters)
            {
                string value = String.Empty;
                if (settings.TryGetSetting(parameter, out recordingSetting))
                {
                    value = Convert.ToString(recordingSetting);
                }

                result = result.Replace($"{{{parameter}}}", value);
            }

            return result;
        }
    }
}
