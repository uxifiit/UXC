/**
 * UXC.Plugins.UXR
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Plugins.UXR.Models.Uploads
{
    static class Directories
    {
        public const string UploadFolderName = "upload";

        private static readonly string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static readonly string LocalAppDataFolderPath = Path.Combine(LocalAppData, Assembly.GetEntryAssembly().GetName().Name);

        public static readonly string UploadFolderPath = Path.Combine(LocalAppDataFolderPath, UploadFolderName);

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
