/**
 * UXC.Core
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
using System.Text;
using System.Threading.Tasks;
using log4net;
using Ninject.Modules;
using UXC.Core.Logging;
using UXC.Logging;
using UXC.Models.Contexts;
using UXI.Common;

namespace UXC.Core.Modules
{
    public class LoggingModule : NinjectModule
    {
        private const string CONFIGURATION_FILENAME = "log4net.config";

        public override void Load()
        {
            Rebind<ILogger>().ToMethod(ctx => new Log4NetLogger(LogManager.GetLogger(ctx.Request.Target?.Member.ReflectedType ?? typeof(LoggingModule))));

            log4net.Config.XmlConfigurator.Configure();

            //string configFilePath = Path.Combine(Path.GetDirectoryName(Locations.EntryAssemblyLocationPath), CONFIGURATION_FILENAME);
            //if (System.IO.File.Exists(configFilePath))
            //{
            //    log4net.Config.XmlConfigurator.Configure(new FileInfo(configFilePath));
            //}
            //else
            //{
            //    log4net.Config.BasicConfigurator.Configure();
            //}
        }
    }
}
