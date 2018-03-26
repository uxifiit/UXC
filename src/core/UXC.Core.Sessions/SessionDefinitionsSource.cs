using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common;
using UXI.Common.Extensions;
using UXC.Core.Modules;
using System.Threading;

namespace UXC.Sessions
{
    public sealed class SessionDefinitionsSource : ISessionDefinitionsSource
    {
        private readonly List<SessionDefinition> _definitions = new List<SessionDefinition>();
        private readonly List<ISessionDefinitionsSource> _linkedCollections = new List<ISessionDefinitionsSource>();

        public SessionDefinitionsSource(IEnumerable<ISessionDefinitionsSource> collections, IModulesService modules)
        {
            collections?.ForEach(Link);

            modules.Register<ISessionDefinitionsSource>(this, c => c?.ForEach(Link));
        }


        public IEnumerable<SessionDefinition> Definitions => _definitions.Concat(_linkedCollections.SelectMany(c => c.Definitions));


        public event EventHandler<CollectionChangedEventArgs<SessionDefinition>> DefinitionsChanged;


        //public void AddRange(IEnumerable<SessionDefinition> definitions)
        //{
        //    var newDefinitions = definitions.Where(d => _definitions.Contains(d) == false).ToList();

        //    if (newDefinitions.Any())
        //    {
        //        _definitions.AddRange(newDefinitions);

        //        DefinitionsChanged?.Invoke(this, CollectionChangedEventArgs<SessionDefinition>.CreateForAddedCollection(definitions));
        //    }
        //}


        //public void Add(SessionDefinition definition)
        //{
        //    if (_definitions.Contains(definition) == false)
        //    {
        //        _definitions.Add(definition);

        //        DefinitionsChanged?.Invoke(this, CollectionChangedEventArgs<SessionDefinition>.CreateForAddedItem(definition));
        //    }
        //}


        //public void Delete(SessionDefinition session)
        //{
        //    if (_definitions.Remove(session))
        //    {
        //        DefinitionsChanged?.Invoke(this, CollectionChangedEventArgs<SessionDefinition>.CreateForRemovedItem(session));
        //    }
        //}


        public void Link(ISessionDefinitionsSource collection)
        {
            collection.ThrowIfNull(nameof(collection));

            if (_linkedCollections.Contains(collection) == false)
            {
                _linkedCollections.Add(collection);

                collection.DefinitionsChanged += LinkedCollection_DefinitionsChanged;

                if (collection.Definitions.Any())
                {
                    DefinitionsChanged?.Invoke(this, CollectionChangedEventArgs<SessionDefinition>.CreateForAddedCollection(collection.Definitions));
                }
            }
        }


        private void LinkedCollection_DefinitionsChanged(object sender, CollectionChangedEventArgs<SessionDefinition> e)
        {
            DefinitionsChanged?.Invoke(this, e);
        }


        public void Unlink(ISessionDefinitionsSource collection)
        {
            if (collection != null && _linkedCollections.Contains(collection))
            {
                _linkedCollections.Remove(collection);

                collection.DefinitionsChanged -= LinkedCollection_DefinitionsChanged;

                if (collection.Definitions.Any())
                {
                    DefinitionsChanged?.Invoke(this, CollectionChangedEventArgs<SessionDefinition>.CreateForRemovedCollection(collection.Definitions));
                }
            }
        }


        public async Task RefreshAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(_linkedCollections.Select(c => c.RefreshAsync(cancellationToken)));
        }
    }
}
