using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Models.Contexts;

namespace UXC.ViewModels
{
    public class AboutViewModel
    {
        private readonly IAppContext _context;

        public AboutViewModel(IAppContext context)
        {
            _context = context;
        }


        public string VersionNumber => _context.Version;


        public bool IsDebugBuild => _context.IsInDebug;


        public bool HasAdminPrivileges => _context.HasAdminPrivileges;
    }
}
