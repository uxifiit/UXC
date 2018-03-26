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

namespace UXC.Sessions
{
    [ConfigurationSection("Sessions")]
    public class SessionsConfiguration : ConfigurationBase, ISessionsConfiguration, IConfiguration
    {
        public SessionsConfiguration(IConfigurationSource source) : base(source)
        {
            TargetPathProperty = CreateProperty(nameof(TargetPath), DEFAULT_TargetPath, source);
        }

        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield return StorageDefinition.Create("sessions.ini");
            }
        }

        public IEnumerable<IConfigurationSettingProperty> Settings
        {
            get
            {
                yield return TargetPathProperty;
            }
        }

        //protected override void OnSettingMissing(string sectionName, string settingKey)
        //{
        //    logger.Error("Missing key: " + settingKey);
        //    base.OnSettingMissing(sectionName, settingKey);
        //}


        private static readonly string DEFAULT_TargetPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), System.Reflection.Assembly.GetEntryAssembly().GetName().Name, "recordings");
        [global::System.Configuration.Setting]
        public string TargetPath
        {
            get
            {
                return TargetPathProperty.Get<string>();
            }
        }
        private readonly ConfigurationSettingProperty TargetPathProperty;
    }
}
