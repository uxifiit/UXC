/**
 * UXC
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;

namespace UXC.Common.Extensions
{
    internal static class ParserEx
    {
        private static Delegate WrapToTypedAction(Action<object> action, Type actionArgType)
        {
            ParameterExpression parameter = Expression.Parameter(actionArgType, "result");
            Expression converted = Expression.Convert(parameter, typeof(object));
            Expression instance = Expression.Constant(action.Target);
            Expression call = Expression.Call(instance, action.Method, converted);

            return Expression.Lambda(typeof(Action<>).MakeGenericType(actionArgType), call, parameter).Compile();
        }

        internal static void ParseArguments(this Parser parser, IEnumerable<string> args, Type optionsType, Action<object> withParsed, Action<IEnumerable<Error>> withNotParsed)
        {
            var withParsedOptions = WrapToTypedAction(withParsed, optionsType);

            parser.CallGenericMethod<object>(nameof(Parser.ParseArguments), optionsType, args)
                  .CallGenericExtensionMethod<object>(typeof(ParserResultExtensions), nameof(ParserResultExtensions.WithParsed), optionsType, withParsedOptions)
                  .CallGenericExtensionMethod<object>(typeof(ParserResultExtensions), nameof(ParserResultExtensions.WithNotParsed), optionsType, withNotParsed);
        }
    }
}
