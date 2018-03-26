using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;
using UXI.Configuration;
using UXI.Configuration.Settings;
using UXI.Configuration.Storages;

namespace UXC.Sessions
{
    public class SessionRecordingSettings
    {
        private const string SETTINGS_SECTION_SESSION = "Session";
        private const char SECTION_KEY_DELIMITER = '.';
        private readonly RuntimeStorage _storage = new RuntimeStorage(SETTINGS_SECTION_SESSION);

        public SessionRecordingSettings(string id, DateTime openedAt)
        {
            ISettings sessionSection;
            _storage.TryGetSection(SETTINGS_SECTION_SESSION, out sessionSection);

            StartedAtProperty = new ConfigurationSettingProperty("StartedAt", typeof(DateTime), DateTime.MinValue, sessionSection);
            FinishedAtProperty = new ConfigurationSettingProperty("FinishedAt", typeof(DateTime), DateTime.MinValue, sessionSection);
            OpenedAtProperty = new ConfigurationSettingProperty("OpenedAt", openedAt, sessionSection);
            IdProperty = new ConfigurationSettingProperty("Id", id, sessionSection);
        }

        internal ConfigurationSettingProperty IdProperty { get; } 

        internal ConfigurationSettingProperty OpenedAtProperty { get; }

        internal ConfigurationSettingProperty StartedAtProperty { get; }

        internal ConfigurationSettingProperty FinishedAtProperty { get; }


        public void SetCustomSetting(string section, string key, object value)
        {
            section.ThrowIf(s => s.Equals(SETTINGS_SECTION_SESSION), nameof(section), "Cannot set session settings");
            _storage.Write(section, key, value);
        }


        public bool TryGetSetting(string section, string key, out object value)
        {
            return _storage.TryRead(section, key, typeof(object), out value);
        }


        public bool TryGetSetting(string name, out object value)
        {
            if (String.IsNullOrWhiteSpace(name) == false)
            {
                int indexOfDelimiter = name.IndexOf(SECTION_KEY_DELIMITER);
                if (indexOfDelimiter > 0 && indexOfDelimiter < name.Length - 1)
                {
                    string section = name.Substring(0, indexOfDelimiter);
                    string key = name.Substring(indexOfDelimiter + 1);

                    if (String.IsNullOrWhiteSpace(section) == false
                        && String.IsNullOrWhiteSpace(key) == false)
                    {
                        return TryGetSetting(section, key, out value);
                    }
                }
            }
            value = null;
            return false;
        }

        public Dictionary<string, Dictionary<string, object>> DumpSettings()
        {
            return _storage.ToDictionary();
        }
    }
}
