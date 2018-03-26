using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Models.Contexts
{
    public interface IAppContext
    {
        string Version { get; }

        bool HasAdminPrivileges { get; }

        bool IsInDebug { get; }

        bool IsDesign { get; }
    }
}
