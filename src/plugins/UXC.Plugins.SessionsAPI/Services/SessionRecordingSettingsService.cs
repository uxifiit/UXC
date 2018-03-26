using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Sessions;
using UXI.Common.Extensions;

namespace UXC.Plugins.SessionsAPI.Services
{
    public class SessionRecordingSettingsService
    {
        private readonly ISessionsControl _control;

        public SessionRecordingSettingsService(ISessionsControl control)
        {
            _control = control;
        }


        public object GetSetting(string section, string key)
        {
            section.ThrowIf(String.IsNullOrWhiteSpace, nameof(section));
            key.ThrowIf(String.IsNullOrWhiteSpace, nameof(key));

            SessionRecording recording = _control.CurrentRecording;    
            object value;

            return recording != null && recording.Settings.TryGetSetting(section, key, out value)
                 ? value
                 : null;
        }


        public bool TrySetSetting(string section, string key, string value)
        {
            section.ThrowIf(String.IsNullOrWhiteSpace, nameof(section));
            key.ThrowIf(String.IsNullOrWhiteSpace, nameof(key));

            SessionRecording recording = _control.CurrentRecording;

            if (recording != null)
            {
                recording.Settings.SetCustomSetting(section, key, value);
                return true;
            }

            return false;
        }
    }
}
