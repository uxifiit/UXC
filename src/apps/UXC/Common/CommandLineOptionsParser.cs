/**
 * UXC
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXC.Common.Extensions;
using UXC.Core.Common;

namespace UXC.Common
{
    /// <summary>
    /// Parses options from command line arguments based on the given instances of <see cref="IOptionsTarget"/> interface. 
    /// </summary>
    public class CommandLineOptionsParser
    {
        private readonly IEnumerable<IOptionsTarget> _targets;


        public CommandLineOptionsParser(IEnumerable<IOptionsTarget> targets)
        {
            _targets = targets?.ToList() ?? Enumerable.Empty<IOptionsTarget>();
        }


        public void Parse(IEnumerable<string> args)
        {
            Parse(args, _targets);
        }


        public static void Parse(IEnumerable<string> args, IEnumerable<IOptionsTarget> targets)
        {
            var parser = new Parser(s =>
            {
                s.CaseSensitive = false;
                s.ParsingCulture = CultureInfo.GetCultureInfo("en-US");
                s.CaseInsensitiveEnumValues = true;
            });

            // use when non generic method exists in the library without forcing to use verbs.
            //var optionsTypes = _targets.Select(t => t.OptionsType);

            // TODO add print help text
            //var errors = new List<Error>();

            foreach (var target in targets)
            {
                // ideally we would use
                //parser.ParseArguments(args, target.OptionsType)
                //      .WithParsed(target.Receive)
                //      .WithNotParsed((errors) => { });


                // but that does not work, we use our extension method
                parser.ParseArguments(args, target.OptionsType, target.ReceiveOptions, _ => { } /* errors.AddRange*/);
            }

            //if (errors.Any())
            //{
            //    foreach (var error in errors.Distinct())
            //    {

            //    }
            //}

            //var helpText = HelpText.AutoBuild(options);
            //helpText.AddEnumValuesToHelpText = true;
            //helpText.AddOptions(options);
        }
    }
}
