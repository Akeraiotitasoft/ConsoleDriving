// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Akeraiotitasoft.ConsoleDriving
{
    /// <summary>
    /// Used to create the <see cref="IConsoleDriverBuilder"/>
    /// </summary>
    public static class ConsoleDriver
    {
        /// <summary>
        /// Creates the default console driver builder
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <returns>An instance of the console driver builder that implements <see cref="IConsoleDriverBuilder"/></returns>
        public static IConsoleDriverBuilder CreateDefaultBuilder(string[] args)
        {
            ConsoleDriverBuilder builder = new (args);
            builder.ConfigureDefaults(args);
            return builder;
        }
    }
}
