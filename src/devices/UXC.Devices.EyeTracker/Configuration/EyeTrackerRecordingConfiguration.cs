using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Configuration;
using UXI.Configuration;

namespace UXC.Devices.EyeTracker
{
    internal class EyeTrackerRecordingConfiguration : IConfiguration
    {
        public EyeTrackerRecordingConfiguration()
        {
            FrequencyProperty = new ConfigurationSettingProperty(nameof(Frequency), typeof(double), DEFAULT_Frequency);
        }

        public IEnumerable<IConfigurationSettingProperty> Settings
        {
            get
            {
                yield return FrequencyProperty;
            }
        }

        public const double DEFAULT_Frequency = 60d;


        public double? Frequency
        {
            get
            {
                if (FrequencyProperty.IsSet)
                {
                    FrequencyProperty.Get<double>();
                }
                return null;
            }
            set
            {
                FrequencyProperty.Set(value);
            }
        }
        private readonly ConfigurationSettingProperty FrequencyProperty;
    }
}
