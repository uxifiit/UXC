/**
 * UXC.Devices.Streamers
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
using UXI.Common.Helpers;
using UXI.Configuration;
using UXI.Common;
using UXI.Configuration.Attributes;
using UXC.Core.Configuration;
using UXI.Configuration.Storages;

namespace UXC.Devices.Streamers.Configuration
{
    [ConfigurationSection(ConfigurationSections.SECTION_DEFAULT)]
    public class StreamersConfiguration : ConfigurationBase, IStreamersConfiguration, IConfiguration
    {
        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield return StorageDefinition.Create("UXC.Devices.Streamers.ini"); // Locations.CallingAssemblyLocationPath);
            }
        }

        public StreamersConfiguration(IConfigurationSource source) : base(source)
        {
            LogOutputProperty = CreateProperty(nameof(LogOutput), DEFAULT_LogOutput, source);
            ShowOutputProperty = CreateProperty(nameof(ShowOutput), DEFAULT_ShowOutput, source);
            StopRecordingTimeoutMillisecondsProperty = CreateProperty(nameof(StopRecordingTimeoutMilliseconds), DEFAULT_StopRecordingTimeoutMilliseconds, source);
            AutoSelectDeviceProperty = CreateProperty(nameof(AutoSelectDevice), DEFAULT_AutoSelectDevice, source);
            FFmpegStartRecordingArgsProperty = CreateProperty(nameof(FFmpegStartRecordingArgs), DEFAULT_FFmpegStartRecordingArgs, source);
        }


        public const bool DEFAULT_LogOutput = false;
        public bool LogOutput { get { return LogOutputProperty.Get<bool>(); } }
        private readonly ConfigurationSettingProperty LogOutputProperty;


        public const bool DEFAULT_ShowOutput = false;
        public bool ShowOutput { get { return ShowOutputProperty.Get<bool>(); } }
        private readonly ConfigurationSettingProperty ShowOutputProperty;


        public const int DEFAULT_StopRecordingTimeoutMilliseconds = 8000;
        public int StopRecordingTimeoutMilliseconds { get { return StopRecordingTimeoutMillisecondsProperty.Get<int>(); } }
        private readonly ConfigurationSettingProperty StopRecordingTimeoutMillisecondsProperty;


        public const bool DEFAULT_AutoSelectDevice = true;
        public bool AutoSelectDevice { get { return AutoSelectDeviceProperty.Get<bool>(); } }
        private readonly ConfigurationSettingProperty AutoSelectDeviceProperty;

        public const string DEFAULT_FFmpegStartRecordingArgs = "-rtbufsize 1500M -f dshow -i {streamType}=\"{deviceName}\" {deviceArgs} {targetPath}";
        /// <summary>
        /// Gets arguments list used when starting recording with FFmpeg. Use placeholders to insert values when assembling the final arguments list. 
        /// Allowed placeholders are {streamType}, {deviceName}, {deviceArgs}, and {targetUrl}.
        /// </summary>
        public string FFmpegStartRecordingArgs { get { return FFmpegStartRecordingArgsProperty.Get<string>(); } }
        private readonly ConfigurationSettingProperty FFmpegStartRecordingArgsProperty;


        public IEnumerable<IConfigurationSettingProperty> Settings
        {
            get
            {
                yield return AutoSelectDeviceProperty;
                yield return StopRecordingTimeoutMillisecondsProperty;
                yield return FFmpegStartRecordingArgsProperty;
                yield return ShowOutputProperty;
                yield return LogOutputProperty;
            }
        }

    }
}
