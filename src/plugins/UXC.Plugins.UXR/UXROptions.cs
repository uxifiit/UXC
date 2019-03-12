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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace UXC.Plugins.UXR
{
 //   [Verb("options")]   // hack, use the verb "options" in every options in the app
    public class UXROptions
    {
        [Option('n', "node-name", HelpText = "Node name.", Required = false)]
        public string NodeName { get; set; }
    }
}
