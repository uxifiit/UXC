using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Sessions.Models
{
    internal static class Directories
    {
        private static readonly string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly string LocalAppDataFolderPath = Path.Combine(LocalAppData, Assembly.GetEntryAssembly().GetName().Name);

        public static string EnsureDirectoryExists(string directoryPath)
        {
            if (Directory.Exists(directoryPath) == false)
            {
                Directory.CreateDirectory(directoryPath);
            }

            return directoryPath;
        }
    }
}
