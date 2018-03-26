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
    class LocalSessionDefinitions : ISessionDefinitionsSource
    {
        private static readonly string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string LocalAppDataFolderPath = Path.Combine(LocalAppData, Assembly.GetEntryAssembly().GetName().Name);

        private const string DEFINITIONS_FOLDER = "definitions";

        private readonly ILocalSessionDefinitionsService _service;
        private Dictionary<string, SessionDefinition> _definitions = null;

        public LocalSessionDefinitions(ILocalSessionDefinitionsService service)
        {
            _service = service;
            //var watcher = new System.IO.FileSystemWatcher(Path.Combine(LocalAppDataFolderPath, DEFINITIONS_FOLDER), "*.json");
        }


        public IEnumerable<SessionDefinition> Definitions => _definitions?.Values.OrderBy(d => d.CreatedAt) ?? Enumerable.Empty<SessionDefinition>();


        public async Task RefreshAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var files = Directory.EnumerateFiles(Path.Combine(LocalAppDataFolderPath, DEFINITIONS_FOLDER), "*.json")
                               .Select(file => new FileInfo(file));

            if (files.Any())
            {
                Dictionary<string, SessionDefinition> currentDefinitions = await Task.Run(() => LoadSessionsDefinitionsFiles(files, cancellationToken), cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                UpdateDefinitions(currentDefinitions);
            }
        }

        private Dictionary<string, SessionDefinition> LoadSessionsDefinitionsFiles(IEnumerable<FileInfo> files, CancellationToken cancellationToken)
        {
            var definitions = new Dictionary<string, SessionDefinition>();

            foreach (var file in files)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var definition = _service.LoadFromFile(file.FullName);
                    definition.CreatedAt = file.LastWriteTime;

                    definitions.Add(file.FullName, definition);
                }
                catch (Exception ex)
                {
                    // TODO LOG failed loading session definition from file
                }
            }

            return definitions;
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
