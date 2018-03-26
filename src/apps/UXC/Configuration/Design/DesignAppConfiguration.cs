using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Configuration.Design
{
    class DesignAppConfiguration : IAppConfiguration
    {
        public bool Experimental
        {
            get { return false; } set { }
        }

        public bool HideOnClose
        {
            get { return true; } set { }
        }
    }
}
