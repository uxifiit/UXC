using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Configuration;
using UXI.Configuration;
using UXI.Configuration.Attributes;

namespace UXC.Devices.Streamers.Configuration
{


    [ConfigurationSection(ConfigurationSections.SECTION_SCREENCAST)]
    public class ScreenCastStreamerConfiguration : ConfigurationBase, IScreenCastStreamerConfiguration, IConfiguration
    {
        public ScreenCastStreamerConfiguration(IVideoStreamerConfiguration videoConfiguration, IStreamersConfiguration streamersConfiguration, IConfigurationSource source) 
            : base(source)
        {
            DeviceNameProperty = CreateProperty(nameof(DeviceName), DEFAULT_DeviceName, source);

            if (videoConfiguration is IConfiguration)
            {
                var videoConfig = videoConfiguration as IConfiguration;
                var videoSettings = videoConfig.Settings.ToDictionary(s => s.Key);

                FFmpegCodecArgsProperty = new ConfigurationSettingProperty(nameof(FFmpegCodecArgs), videoSettings[nameof(videoConfiguration.FFmpegCodecArgs)], DefaultSection);
                StopRecordingTimeoutMillisecondsProperty = new ConfigurationSettingProperty(nameof(StopRecordingTimeoutMilliseconds), videoSettings[nameof(videoConfiguration.StopRecordingTimeoutMilliseconds)], DefaultSection);
                ExtensionProperty = new ConfigurationSettingProperty(nameof(Extension), videoSettings[nameof(videoConfiguration.Extension)], DefaultSection);
            }
            else
            {
                FFmpegCodecArgsProperty = new ConfigurationSettingProperty(nameof(FFmpegCodecArgs), typeof(string), videoConfiguration.FFmpegCodecArgs, DefaultSection);
                StopRecordingTimeoutMillisecondsProperty = new ConfigurationSettingProperty(nameof(StopRecordingTimeoutMilliseconds), typeof(int), videoConfiguration.StopRecordingTimeoutMilliseconds, DefaultSection);
                ExtensionProperty = new ConfigurationSettingProperty(nameof(Extension), typeof(string), videoConfiguration.Extension, DefaultSection);
            }


            if (streamersConfiguration is IConfiguration)
            {
                var streamersConfig = streamersConfiguration as IConfiguration;
                var streamersSettings = streamersConfig.Settings.ToDictionary(s => s.Key);

                AutoSelectDeviceProperty = new ConfigurationSettingProperty(nameof(AutoSelectDevice), streamersSettings[nameof(streamersConfiguration.AutoSelectDevice)], DefaultSection);
            }
            else
            {
                AutoSelectDeviceProperty = new ConfigurationSettingProperty(nameof(AutoSelectDevice), typeof(bool), streamersConfiguration.AutoSelectDevice, DefaultSection);
            }
        }


        public const string DEFAULT_DeviceName = "UScreenCapture";
        public string DeviceName { get { return DeviceNameProperty.Get<string>(); } }
        private readonly ConfigurationSettingProperty DeviceNameProperty;


        public const bool DEFAULT_AutoSelect = false;
        public bool AutoSelectDevice { get { return AutoSelectDeviceProperty.Get<bool>(); } }
        private readonly ConfigurationSettingProperty AutoSelectDeviceProperty;


        public string FFmpegCodecArgs { get { return FFmpegCodecArgsProperty.Get<string>(); } }
        private readonly ConfigurationSettingProperty FFmpegCodecArgsProperty;


        public string Extension { get { return ExtensionProperty.Get<string>(); } }
        private readonly ConfigurationSettingProperty ExtensionProperty;


        public int StopRecordingTimeoutMilliseconds { get { return StopRecordingTimeoutMillisecondsProperty.Get<int>(); } }
        private readonly ConfigurationSettingProperty StopRecordingTimeoutMillisecondsProperty;


        public IEnumerable<IConfigurationSettingProperty> Settings
        {
            get
            {
                yield return DeviceNameProperty;
                yield return AutoSelectDeviceProperty;
                yield return FFmpegCodecArgsProperty;
                yield return ExtensionProperty;
                yield return StopRecordingTimeoutMillisecondsProperty;
            }
        }
    }
}
