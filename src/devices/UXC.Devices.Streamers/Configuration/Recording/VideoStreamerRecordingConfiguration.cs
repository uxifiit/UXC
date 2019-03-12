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
using UXC.Core.Configuration;
using UXI.Common.Extensions;
using UXI.Configuration;

namespace UXC.Devices.Streamers.Configuration
{
    internal class VideoStreamerRecordingConfiguration : StreamerRecordingConfiguration
    {
        public VideoStreamerRecordingConfiguration(IVideoStreamerConfiguration configuration)
        {
            if (configuration is IConfiguration)
            {
                var config = (IConfiguration)configuration;
                var settings = config.Settings.ToDictionary(s => s.Key);

                StopRecordingTimeoutMillisecondsProperty = new ConfigurationSettingProperty(nameof(StopRecordingTimeoutMilliseconds), settings[nameof(IVideoStreamerConfiguration.StopRecordingTimeoutMilliseconds)]);
            }
            else
            {
                StopRecordingTimeoutMillisecondsProperty = new ConfigurationSettingProperty(nameof(StopRecordingTimeoutMilliseconds), typeof(int), configuration.StopRecordingTimeoutMilliseconds);
            }

            //StopRecordingTimeoutMillisecondsProperty = new ConfigurationSettingProperty(

        }

        public void ApplyPresetSettings(VideoStreamerRecordingPreset preset)
        {
            preset.ThrowIfNull(nameof(preset));

            //ResolutionProperty.Set(preset.Resolution);
            //FrameRateProperty.Set(preset.FrameRate);
            //BitrateProperty.Set(preset.Bitrate);
        }

   
        public string Preset
        {
            get { return PresetProperty.Get<string>(); }
            set { PresetProperty.Set(value); }
        }
        private readonly ConfigurationSettingProperty PresetProperty = new ConfigurationSettingProperty(nameof(Preset), typeof(string), defaultValue: null);



        public int StopRecordingTimeoutMilliseconds { get { return StopRecordingTimeoutMillisecondsProperty.Get<int>(); } }
        private readonly ConfigurationSettingProperty StopRecordingTimeoutMillisecondsProperty;

        //public int FrameRate
        //{
        //    get { return FrameRateProperty.Get<int>(); }
        //    set { FrameRateProperty.Set(value); }
        //}
        //private readonly ConfigurationSettingProperty FrameRateProperty = new ConfigurationSettingProperty(nameof(FrameRate), typeof(int), DEFAULT_FrameRate_HDR);

        //public int Bitrate
        //{
        //    get { return BitrateProperty.Get<int>(); }
        //    set { BitrateProperty.Set(value); }
        //}
        //private readonly ConfigurationSettingProperty BitrateProperty = new ConfigurationSettingProperty(nameof(Bitrate), typeof(int), DEFAULT_Bitrate_HDR);

        //public string Resolution
        //{
        //    get { return ResolutionProperty.Get<string>(); }
        //    set { ResolutionProperty.Set(value); }
        //}
        //private readonly ConfigurationSettingProperty ResolutionProperty = new ConfigurationSettingProperty(nameof(Resolution), typeof(string), DEFAULT_Resolution_HDR);


        public override IEnumerable<IConfigurationSettingProperty> Settings
        {
            get
            {
                foreach (var setting in base.Settings)
                {
                    yield return setting;
                }

                yield return PresetProperty;
                yield return StopRecordingTimeoutMillisecondsProperty;
                //yield return ResolutionProperty;
                //yield return FrameRateProperty;
                //yield return BitrateProperty;
            }
        }
    }
}
