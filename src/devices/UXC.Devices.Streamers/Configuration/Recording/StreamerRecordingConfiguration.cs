using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Configuration;
using UXI.Configuration;

namespace UXC.Devices.Streamers.Configuration
{
    internal class StreamerRecordingConfiguration : IConfiguration
    {
        public string TargetPath
        {
            get { return TargetPathProperty.Get<string>(); }
            set { TargetPathProperty.Set(value); }
        }
        private readonly ConfigurationSettingProperty TargetPathProperty = new ConfigurationSettingProperty(nameof(TargetPath), typeof(string), String.Empty);
        public virtual IEnumerable<IConfigurationSettingProperty> Settings
        {
            get
            {
                yield return TargetPathProperty;
            }
        }
    }
}
