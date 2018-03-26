using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Configuration;
using UXI.Common;
using UXI.Configuration;
using UXI.Configuration.Attributes;
using UXI.Configuration.Storages;

namespace UXC.Plugins.UXR.Configuration
{
    [ConfigurationSection("UXR")]
    public class UXRConfiguration : ConfigurationBase, IConfiguration, IUXRConfiguration
    {
        public UXRConfiguration(IConfigurationSource source) : base(source)
        {
            NodeNameProperty = CreateProperty(nameof(NodeName), DEFAULT_NodeName, source);
            EndpointAddressProperty = CreateProperty(nameof(EndpointAddress), DEFAULT_EndpointAddress, source);
            StatusUpdateIntervalSecondsProperty = CreateProperty(nameof(StatusUpdateIntervalSeconds), DEFAULT_StatusUpdateIntervalSeconds, source);
            UploadTimeoutSecondsProperty = CreateProperty(nameof(UploadTimeoutSeconds), DEFAULT_UploadTimeoutSeconds, source);
        }

        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield return StorageDefinition.Create("UXC.Plugins.UXR.ini"); // Locations.CallingAssemblyLocationPath);
            }
        }

        public IEnumerable<IConfigurationSettingProperty> Settings
        {
            get
            {
                yield return NodeNameProperty;
                yield return StatusUpdateIntervalSecondsProperty;
                yield return EndpointAddressProperty;
                yield return UploadTimeoutSecondsProperty;
            }
        }

        //protected override void OnSettingMissing(string sectionName, string settingKey)
        //{
        //    logger.Error("Missing key: " + settingKey);
        //    base.OnSettingMissing(sectionName, settingKey);
        //}


        private static readonly string DEFAULT_EndpointAddress = "http://localhost:15185";
        [global::System.Configuration.Setting]
        public string EndpointAddress
        {
            get { return EndpointAddressProperty.Get<string>(); }
            set { EndpointAddressProperty.Set(value); }
        }
        private readonly ConfigurationSettingProperty EndpointAddressProperty;



        private static readonly int DEFAULT_StatusUpdateIntervalSeconds = 5;
        [global::System.Configuration.Setting]
        public int StatusUpdateIntervalSeconds
        {
            get { return StatusUpdateIntervalSecondsProperty.Get<int>(); }
            set { StatusUpdateIntervalSecondsProperty.Set(value); }
        }
        private readonly ConfigurationSettingProperty StatusUpdateIntervalSecondsProperty;



        private static readonly string DEFAULT_NodeName = String.Empty;
        [global::System.Configuration.Setting]
        public string NodeName
        {
            get { return NodeNameProperty.Get<string>(); }
            set { NodeNameProperty.Set(value); }
        }
        private readonly ConfigurationSettingProperty NodeNameProperty;


        private static readonly int DEFAULT_UploadTimeoutSeconds = 30;
        [global::System.Configuration.Setting]
        public int UploadTimeoutSeconds
        {
            get { return UploadTimeoutSecondsProperty.Get<int>(); }
            set { UploadTimeoutSecondsProperty.Set(value); }
        }
        private readonly ConfigurationSettingProperty UploadTimeoutSecondsProperty;
    }
}
