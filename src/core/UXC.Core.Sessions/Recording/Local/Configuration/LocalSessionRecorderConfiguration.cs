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
using UXC.Core.Configuration;
using UXI.Configuration;

namespace UXC.Sessions.Recording.Local.Configuration
{
    class LocalSessionRecorderConfiguration : IConfiguration
    {
        public LocalSessionRecorderConfiguration(ISessionsConfiguration configuration)
        {
            CreateSessionFolderProperty = new ConfigurationSettingProperty(nameof(CreateSessionFolder), typeof(bool), DEFAULT_CreateSessionFolder);

            var settings = configuration.Settings.ToDictionary(s => s.Key);
            if (settings.ContainsKey(nameof(ISessionsConfiguration.TargetPath)))
            {
                TargetPathProperty = new ConfigurationSettingProperty(nameof(TargetPath), settings[nameof(ISessionsConfiguration.TargetPath)]);
            }
            else
            {
                TargetPathProperty = new ConfigurationSettingProperty(nameof(TargetPath), typeof(string), configuration.TargetPath);
            }
        }


        public IEnumerable<IConfigurationSettingProperty> Settings
        {
            get
            {
                yield return TargetPathProperty;
            }
        }

        //protected override void OnSettingMissing(string sectionName, string settingKey)
        //{
        //    logger.Error("Missing key: " + settingKey);
        //    base.OnSettingMissing(sectionName, settingKey);
        //}


        [global::System.Configuration.Setting]
        public string TargetPath
        {
            get
            {
                return TargetPathProperty.Get<string>();
            }
        }
        private readonly ConfigurationSettingProperty TargetPathProperty;


        private const bool DEFAULT_CreateSessionFolder = true;
        [global::System.Configuration.Setting]
        public bool CreateSessionFolder
        {
            get
            {
                return CreateSessionFolderProperty.Get<bool>();
            }
        }
        private readonly ConfigurationSettingProperty CreateSessionFolderProperty;
    }
}
