using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UXI.Common;

namespace UXC.Sessions
{
    public interface ISessionDefinitionsSource
    {
        IEnumerable<SessionDefinition> Definitions { get; }

        event EventHandler<CollectionChangedEventArgs<SessionDefinition>> DefinitionsChanged;

        Task RefreshAsync(CancellationToken cancellationToken);
    }
}
