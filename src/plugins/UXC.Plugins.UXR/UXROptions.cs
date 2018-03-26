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
