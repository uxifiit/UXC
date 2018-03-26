using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UXC.Plugins.UXR.Models.Uploads
{
    public class UploadsSource : IUploadsSource
    {
        private const string UPLOADS_FILE = "uploads.json";


        private static readonly string UploadsFilePath = Path.Combine(Directories.LocalAppDataFolderPath, UPLOADS_FILE);

        
        private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };


        public IEnumerable<SessionRecordingData> Load()
        {
            if (File.Exists(UploadsFilePath))
            {
                try
                {
                    string uploadsJson = File.ReadAllText(UploadsFilePath);
                    var recordings = JsonConvert.DeserializeObject<List<SessionRecordingData>>(uploadsJson, jsonSettings);

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
                File.WriteAllText(UploadsFilePath, json, new UTF8Encoding(false));

                return true;
            }
            catch
            {

            }
            return false;
        }
    }
}
