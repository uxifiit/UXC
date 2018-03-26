using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Data.Serialization
{
    public interface IDataReader : IDisposable
    {
        bool CanRead(Type objectType);

        bool TryRead(out object data);
    }
}
