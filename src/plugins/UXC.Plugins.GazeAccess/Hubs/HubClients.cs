using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace UXC.Plugins.GazeAccess.Hubs
{
    public class HubClients
    {
        private readonly ConcurrentDictionary<string, int> _clients = new ConcurrentDictionary<string, int>();

        public void Add(string client, int group)
        {
            OnClientAdded(group);

            if (_clients.ContainsKey(client))
            {
                Remove(client);
            }

            bool first = (Count == 0);
            bool added = _clients.TryAdd(client, group);

            if (added && first)
            {
                FirstClientConnected?.Invoke(this, EventArgs.Empty);
            }
        }

        public int Remove(string client)
        {
            int group;
            _clients.TryRemove(client, out group);

            OnClientRemoved(group);

            if (Count == 0)
            {
                LastClientDisconnected?.Invoke(this, EventArgs.Empty);        
            }

            return group;
        }

        public int Update(string client, int group)
        {
            int old = Remove(client);

            Add(client, group);

            return old;
        }

        public bool TryGet(string client, out int group)
        {
            return _clients.TryGetValue(client, out group);
        }

        private bool GroupExists(int group) => _clients.Values.Any(v => v == group);

        private void OnClientAdded(int group)
        {
            if (GroupExists(group) == false)
            {
                GroupCreated?.Invoke(this, group);
            }
        }

        private void OnClientRemoved(int group)
        {
            if (GroupExists(group) == false)
            {
                GroupClosed?.Invoke(this, group);
            }
        }

        public event EventHandler<int> GroupCreated;
        public event EventHandler<int> GroupClosed;

        public event EventHandler FirstClientConnected;
        public event EventHandler LastClientDisconnected;

        public IEnumerable<int> Groups => _clients.Values.Distinct().OrderBy(g => g);
        public int Count => _clients.Count;
    }
}
