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
    [ConfigurationSection(ConfigurationSections.SECTION_VIDEO)]
    public class VideoStreamerConfiguration : ConfigurationBase, IVideoStreamerConfiguration, IConfiguration
    {
        public VideoStreamerConfiguration(IStreamersConfiguration streamersConfiguration, IConfigurationSource source) 
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

        public const string DEFAULT_DeviceName = "Creative Senz3D VF0780";
        public string DeviceName { get { return DeviceNameProperty.Get<string>(); } }
        private readonly ConfigurationSettingProperty DeviceNameProperty;


        public bool AutoSelectDevice { get { return AutoSelectDeviceProperty.Get<bool>(); } }
        private readonly ConfigurationSettingProperty AutoSelectDeviceProperty;


        public const string DEFAULT_FFmpegCodecArgs = "-vcodec libx264 -crf 28 -vf scale={resolutionScale} -pix_fmt yuv422p -an -preset ultrafast";
        public string FFmpegCodecArgs { get { return FFmpegCodecArgsProperty.Get<string>(); } }
        private readonly ConfigurationSettingProperty FFmpegCodecArgsProperty;


        public const string DEFAULT_Extension = "mp4";
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
