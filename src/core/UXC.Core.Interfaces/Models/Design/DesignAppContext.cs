using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Models.Contexts.Design
{
    public class DesignAppContext : IAppContext
    {
        public bool HasAdminPrivileges => false;

        public bool IsInDebug => true;

        public string Version => new Version(0, 1, 1).ToString();

        public bool IsDesign => true;
    }
}
