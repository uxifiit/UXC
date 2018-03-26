using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common;
using UXI.Configuration;
using UXI.Configuration.Attributes;
using UXI.Configuration.Storages;

namespace UXC.Configuration
{
    [ConfigurationSection("Server")]
    internal class ServerConfiguration : ConfigurationBase, IServerConfiguration
    {
        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield return StorageDefinition.Create("Server.ini");//Locations.CallingAssemblyLocationName;
            }
        }


        public ServerConfiguration(IConfigurationSource source) : base(source)
        {
            ServerPortProperty = CreateProperty(nameof(ServerPort), DEFAULT_ServerAddress, source);
            SslEnabledProperty = CreateProperty(nameof(SslEnabled), DEFAULT_SslEnabled, source);
            SignalREnabledProperty = CreateProperty(nameof(SignalREnabled), DEFAULT_SignalREnabled, source);
            CustomHostNameProperty = CreateProperty(nameof(CustomHostName), DEFAULT_CustomHostName, source);
        }

        public IEnumerable<IConfigurationSettingProperty> Properties
        {
            get
            {
                yield return ServerPortProperty;
                yield return SslEnabledProperty;
                yield return SignalREnabledProperty;
            }
        }

        private const uint DEFAULT_ServerAddress = 64328;
        [global::System.Configuration.Setting]
        [global::System.Diagnostics.DebuggerNonUserCode]
        public uint ServerPort
        {
            get
            {
                return ServerPortProperty.Get<uint>();
            }
            set
            {
                ServerPortProperty.Set(value);
            }
        }
        private readonly ConfigurationSettingProperty ServerPortProperty;


        private const bool DEFAULT_SslEnabled = false;
        [global::System.Configuration.Setting]
        [global::System.Diagnostics.DebuggerNonUserCode]
        public bool SslEnabled
        {
            get
            {
                return SslEnabledProperty.Get<bool>();
            }
            set
            {
                SslEnabledProperty.Set(value);
            }
        }
        private readonly ConfigurationSettingProperty SslEnabledProperty;



        private const bool DEFAULT_SignalREnabled = true;
        [global::System.Configuration.Setting]
        [global::System.Diagnostics.DebuggerNonUserCode]
        public bool SignalREnabled
        {
            get
            {
                return SignalREnabledProperty.Get<bool>();
            }
            set
            {
                SignalREnabledProperty.Set(value);
            }
        }
        private readonly ConfigurationSettingProperty SignalREnabledProperty;


        private const string DEFAULT_CustomHostName = "";
        [global::System.Configuration.Setting]
        [global::System.Diagnostics.DebuggerNonUserCode]
        public string CustomHostName
        {
            get
            {
                return CustomHostNameProperty.Get<string>();
            }
            set
            {
                CustomHostNameProperty.Set(value);
            }
        }
        private readonly ConfigurationSettingProperty CustomHostNameProperty;
    }
}
