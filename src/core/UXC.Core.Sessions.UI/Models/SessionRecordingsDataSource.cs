using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UXC.Sessions.Models
{
    public class SessionRecordingsDataSource : ISessionRecordingsDataSource
    {
        private const string SESSION_RECORDINGS_DATA_FILE = "recordings.json";


        private static readonly string SessionRecordingsDataFilePath = Path.Combine(Directories.LocalAppDataFolderPath, SESSION_RECORDINGS_DATA_FILE);

        
        private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };


        public IEnumerable<SessionRecordingData> Load()
        {
            if (File.Exists(SessionRecordingsDataFilePath))
            {
                try
                {
                    string recordingsJson = File.ReadAllText(SessionRecordingsDataFilePath);
                    var recordings = JsonConvert.DeserializeObject<List<SessionRecordingData>>(recordingsJson, jsonSettings);

                    return recordings;
                }
                catch
                {

                }
            }

            return Enumerable.Empty<SessionRecordingData>();
        }


        public bool Save(IEnumerable<SessionRecordingData> recordings)
        {
            try
            {
                Directories.EnsureDirectoryExists(Directories.LocalAppDataFolderPath);

                var json = JsonConvert.SerializeObject(recordings.ToArray(), Formatting.Indented, jsonSettings);
                File.WriteAllText(SessionRecordingsDataFilePath, json, new UTF8Encoding(false));

                return true;
            }
            catch
            {

            }

            return false;
        }
    }
}
