using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Configuration;
using UXI.Configuration.Settings;
using UXI.Configuration.Storages;

namespace UXC.Design.Configuration
{
    public class DesignConfigurationSource : IConfigurationSource
    {
        private readonly List<IStorage> _storages = new List<IStorage>();
        private readonly ConcurrentDictionary<string, DictionarySettings> _sections = new ConcurrentDictionary<string, DictionarySettings>();  
                                            
        public IEnumerable<string> Sections
        {
            get
            {
                return _sections.Keys;
            }
        }

        public IEnumerable<string> AddFile(string path)
        {
            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> AddStorage(IStorage storage)
        {
            foreach (var section in storage.Sections)
            {
                DictionarySettings settings;
                if (_sections.TryGetValue(section, out settings) == false)
                {
                    _sections.TryAdd(section, settings = new DictionarySettings());
                }

                foreach (var key in storage.GetKeys(section))
                {
                    object value;
                    if (storage.TryRead(section, key, typeof(object), out value))
                    {
                        settings.SetSetting(key, value);
                    }
                }
            }

            return storage.Sections;
        }

        public ISettings GetSection(string name)
        {
            return _sections[name];
        }

        public bool HasSection(string name)
        {
            return _sections.ContainsKey(name);
        }
    }
}
