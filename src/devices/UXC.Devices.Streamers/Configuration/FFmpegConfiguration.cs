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

    [ConfigurationSection(ConfigurationSections.SECTION_FFMPEG)]
    public class FFmpegConfiguration : ConfigurationBase, IFFmpegConfiguration, IConfiguration
    {
        public FFmpegConfiguration(IConfigurationSource source) : base(source)
        {
            EnumerateDevicesArgsProperty = CreateProperty(nameof(EnumerateDevicesArgs), DEFAULT_EnumerateDevicesArgs, source);
            FFmpegPathProperty = CreateProperty(nameof(FFmpegPath), DEFAULT_FFmpegPath, source);
            DevicesListHeaderTemplateProperty = CreateProperty(nameof(DevicesListHeaderTemplate), DEFAULT_DevicesListHeaderTemplate, source);
        }


        public const string DEFAULT_EnumerateDevicesArgs = "-list_devices true -f dshow -i dummy";
        public string EnumerateDevicesArgs { get { return EnumerateDevicesArgsProperty.Get<string>(); } }
        private readonly ConfigurationSettingProperty EnumerateDevicesArgsProperty;


        public const string DEFAULT_FFmpegPath = @"lib\ffmpeg.exe";
        public string FFmpegPath { get { return FFmpegPathProperty.Get<string>(); } }
        private readonly ConfigurationSettingProperty FFmpegPathProperty;


        public const string DEFAULT_DevicesListHeaderTemplate = "DirectShow {streamType} devices";
        public string DevicesListHeaderTemplate { get { return DevicesListHeaderTemplateProperty.Get<string>(); } }
        private readonly ConfigurationSettingProperty DevicesListHeaderTemplateProperty;


        public IEnumerable<IConfigurationSettingProperty> Settings
        {
            get
            {
                yield return FFmpegPathProperty;
                yield return EnumerateDevicesArgsProperty;
                yield return DevicesListHeaderTemplateProperty;
            }
        }
    }
}
