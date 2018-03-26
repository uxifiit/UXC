using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Configuration;
using UXI.Configuration;

namespace UXC.Devices.Streamers.Configuration
{
    internal class AudioStreamerRecordingConfiguration : StreamerRecordingConfiguration
    {
        public AudioStreamerRecordingConfiguration(IAudioStreamerConfiguration configuration)
        {
            if (configuration is IConfiguration)
            {
                var config = (IConfiguration)configuration;
                var settings = config.Settings.ToDictionary(s => s.Key);

                StopRecordingTimeoutMillisecondsProperty = new ConfigurationSettingProperty(nameof(StopRecordingTimeoutMilliseconds), settings[nameof(IAudioStreamerConfiguration.StopRecordingTimeoutMilliseconds)]);
            }
            else
            {
                StopRecordingTimeoutMillisecondsProperty = new ConfigurationSettingProperty(nameof(StopRecordingTimeoutMilliseconds), typeof(int), configuration.StopRecordingTimeoutMilliseconds);
            }
        }



        public const int DEFAULT_Bitrate = 256;
        public int Bitrate
        {
            get { return BitrateProperty.Get<int>(); }
            set { BitrateProperty.Set(value); }
        }
        private readonly ConfigurationSettingProperty BitrateProperty = new ConfigurationSettingProperty(nameof(Bitrate), typeof(int), DEFAULT_Bitrate);



        public int StopRecordingTimeoutMilliseconds { get { return StopRecordingTimeoutMillisecondsProperty.Get<int>(); } }
        private readonly ConfigurationSettingProperty StopRecordingTimeoutMillisecondsProperty;




        public override IEnumerable<IConfigurationSettingProperty> Settings
        {
            get
            {
                foreach (var setting in base.Settings)
                {
                    yield return setting;
                }
                yield return BitrateProperty;
                yield return StopRecordingTimeoutMillisecondsProperty;
            }
        }
    }
}
