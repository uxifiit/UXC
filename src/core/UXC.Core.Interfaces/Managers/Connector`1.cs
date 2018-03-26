using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Modules;

namespace UXC.Core.Managers
{
    public class Connector<T> : Connector<T, T>
        where T : class
    {
        public Connector(IManager<T> manager, IEnumerable<T> items, IModulesService modules)
            : base(manager, items, Convert, modules)
        {
        }

        private static T Convert(T item) => item;
    }

    public class Connector<TSource, TResult> : IConnector
        where TSource : class
        where TResult : class
    {
        private readonly Queue<TResult> _queue = new Queue<TResult>();
        private readonly IManager<TResult> _manager;
        private readonly Converter<TSource, TResult> _converter;

        public bool IsConnecting { get; private set; } = false;

        public Connector(IManager<TResult> manager, IEnumerable<TSource> items, Converter<TSource, TResult> converter, IModulesService modules)
        {
            _manager = manager;
            _converter = converter;
            EnqueueOrConnect(items);

            modules.Register<TSource>(this, EnqueueOrConnect);
        }

        private void EnqueueOrConnect(IEnumerable<TSource> items)
        {
            var convertedItems = items.Select(i => _converter.Invoke(i));

            foreach (var item in convertedItems.Where(i => _manager.Contains(i) == false))
            {
                _queue.Enqueue(item);
            }

            if (IsConnecting)
            {
                ConnectAll();
            }
        }

        public void ConnectAll()
        {
            var items = _queue.ToArray();
            _queue.Clear();

            IsConnecting = true;

            _manager.ConnectMany(items);
        }
        //public void DisconnectAll()
        //{
        //    _manager.DisconnectAll();
        //}
    }
}
