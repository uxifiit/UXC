using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Common
{
    internal static class FileHelper
    {
        public static void ShowInExplorer(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
            {
                Process.Start("explorer.exe", $"/select,\"{path}\"");
            }
        }

        public static void OpenDirectoryInExplorer(string path)
        {
            if (Directory.Exists(path))
            {
                Process.Start("explorer.exe", $"\"{path}\"");        
            }
        }
    }
}
