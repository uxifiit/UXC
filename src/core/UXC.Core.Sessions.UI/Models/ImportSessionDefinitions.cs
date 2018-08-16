using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UXI.Common;
using UXI.Common.Extensions;

namespace UXC.Sessions
{
    class ImportSessionDefinitions : ISessionDefinitionsSource
    {
        private readonly ILocalSessionDefinitionsService _service;
        private Dictionary<string, SessionDefinition> _definitions = new Dictionary<string, SessionDefinition>();

        public ImportSessionDefinitions(ILocalSessionDefinitionsService service)
        {
            _service = service;
            //var watcher = new System.IO.FileSystemWatcher(Path.Combine(LocalAppDataFolderPath, DEFINITIONS_FOLDER), "*.json");
        }


        public IEnumerable<SessionDefinition> Definitions => _definitions?.Values.OrderBy(d => d.CreatedAt) ?? Enumerable.Empty<SessionDefinition>();


        public bool TryAdd(string filepath, out SessionDefinition definition)
        {
            FileInfo file = new FileInfo(filepath);

            string key = file.FullName;
            if (file.Exists && TryLoadDefinition(file, out definition))
            {
                if (_definitions.ContainsKey(key) == false)
                {
                    _definitions.Add(key, definition);

                    DefinitionsChanged?.Invoke(this, CollectionChangedEventArgs<SessionDefinition>.CreateForAddedItem(definition));

                    return true;
                }
                else if (_definitions[key].CreatedAt != definition.CreatedAt)
                {
                    var removed = _definitions[key];
                    _definitions.Remove(key);
                    _definitions.Add(key, definition);

                    DefinitionsChanged?.Invoke(this, CollectionChangedEventArgs<SessionDefinition>.Create(definition, removed));

                    return true;
                }
            }
            // TODO add message file does not exist

            definition = null;
            return false;
        }


        public async Task RefreshAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var files = _definitions?.Keys.Select(file => new FileInfo(file));

            if (files != null && files.Any())
            {
                Dictionary<string, SessionDefinition> currentDefinitions = await Task.Run(() => LoadDefinitionFiles(files, cancellationToken), cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                UpdateDefinitions(currentDefinitions);
            }
        }


        private Dictionary<string, SessionDefinition> LoadDefinitionFiles(IEnumerable<FileInfo> files, CancellationToken cancellationToken)
        {
            var definitions = new Dictionary<string, SessionDefinition>();
            SessionDefinition definition;

            foreach (var file in files.Where(f => f.Exists))
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (TryLoadDefinition(file, out definition))
                {
                    definitions.Add(file.FullName, definition);
                }  
            }

            return definitions;
        }


        private bool TryLoadDefinition(FileInfo file, out SessionDefinition definition)
        {
            try
            {
                definition = _service.LoadFromFile(file.FullName);
                definition.CreatedAt = file.LastWriteTime;

                if (definition.Recorders.Any(r => r.Name.Equals("Local", StringComparison.CurrentCultureIgnoreCase)) == false)
                {
                    definition.Recorders.Add(new SessionRecorderDefinition("Local"));
                }

                return true;
            }
            catch (Exception ex)
            {
                // TODO LOG
            }

            definition = null;
            return false;
        }


        private void UpdateDefinitions(Dictionary<string, SessionDefinition> currentDefinitions)
        {
            var previousDefinitions = ObjectEx.GetAndReplace(ref _definitions, currentDefinitions);
            if (previousDefinitions != null && previousDefinitions.Any())
            {
                var addedDefinitions = currentDefinitions.Where(kvp => previousDefinitions.ContainsKey(kvp.Key) == false
                                                                    || previousDefinitions[kvp.Key].CreatedAt != kvp.Value.CreatedAt)
                                                         .Select(kvp => kvp.Value)
                                                         .OrderBy(definition => definition.CreatedAt)
                                                         .ToList();

                var removedDefinitions = previousDefinitions.Where(kvp => currentDefinitions.ContainsKey(kvp.Key) == false
                                                                       || currentDefinitions[kvp.Key].CreatedAt != kvp.Value.CreatedAt)
                                                            .Select(kvp => kvp.Value)
                                                            .ToList();

                DefinitionsChanged?.Invoke(this, CollectionChangedEventArgs<SessionDefinition>.Create(addedDefinitions, removedDefinitions));
            }
            else if (currentDefinitions.Any())
            {
                DefinitionsChanged?.Invoke(this, CollectionChangedEventArgs<SessionDefinition>.CreateForAddedCollection(currentDefinitions.Values.OrderBy(d => d.CreatedAt)));
            }
        }


        public event EventHandler<CollectionChangedEventArgs<SessionDefinition>> DefinitionsChanged;
    }
}
