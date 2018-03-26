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
    [ConfigurationSection(ConfigurationSections.SECTION_AUDIO)]
    public class AudioStreamerConfiguration : ConfigurationBase, IAudioStreamerConfiguration, IConfiguration
    {
        public AudioStreamerConfiguration(IStreamersConfiguration streamersConfiguration, IConfigurationSource source)
            : base(source)
        {
            DeviceNameProperty = CreateProperty(nameof(DeviceName), DEFAULT_DeviceName, source);
            FFmpegCodecArgsProperty = CreateProperty(nameof(FFmpegCodecArgs), DEFAULT_FFmpegCodecArgs, source);
            ExtensionProperty = CreateProperty(nameof(Extension), DEFAULT_Extension, source);


            if (streamersConfiguration is IConfiguration)
            {
                var config = streamersConfiguration as IConfiguration;
                var settings = config.Settings.ToDictionary(s => s.Key);

                AutoSelectDeviceProperty = new ConfigurationSettingProperty(nameof(AutoSelectDevice), settings[nameof(streamersConfiguration.AutoSelectDevice)], DefaultSection);
                StopRecordingTimeoutMillisecondsProperty = new ConfigurationSettingProperty(nameof(StopRecordingTimeoutMilliseconds), settings[nameof(streamersConfiguration.StopRecordingTimeoutMilliseconds)], DefaultSection);
            }
            else
            {
                AutoSelectDeviceProperty = new ConfigurationSettingProperty(nameof(AutoSelectDevice), typeof(bool), streamersConfiguration.AutoSelectDevice, DefaultSection);
                StopRecordingTimeoutMillisecondsProperty = new ConfigurationSettingProperty(nameof(StopRecordingTimeoutMilliseconds), typeof(int), streamersConfiguration.StopRecordingTimeoutMilliseconds, DefaultSection);
            }
        }


        public const string DEFAULT_DeviceName = "Microphone Array (Creative Senz3D VF0780)";
        public string DeviceName { get { return DeviceNameProperty.Get<string>(); } }
        private readonly ConfigurationSettingProperty DeviceNameProperty;


        public const string DEFAULT_FFmpegCodecArgs = "-acodec libmp3lame -b:a {bitrate}k";
        public string FFmpegCodecArgs { get { return FFmpegCodecArgsProperty.Get<string>(); } }
        private readonly ConfigurationSettingProperty FFmpegCodecArgsProperty;


        public int StopRecordingTimeoutMilliseconds { get { return StopRecordingTimeoutMillisecondsProperty.Get<int>(); } }
        private readonly ConfigurationSettingProperty StopRecordingTimeoutMillisecondsProperty;


        public const string DEFAULT_Extension = "mp3";
        public string Extension { get { return ExtensionProperty.Get<string>(); } }
        private readonly ConfigurationSettingProperty ExtensionProperty;


        public bool AutoSelectDevice { get { return AutoSelectDeviceProperty.Get<bool>(); } }
        public readonly ConfigurationSettingProperty AutoSelectDeviceProperty;


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
