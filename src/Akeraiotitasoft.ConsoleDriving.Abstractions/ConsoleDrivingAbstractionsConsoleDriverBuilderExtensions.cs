// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Akeraiotitasoft.ConsoleDriving
{
    /// <summary>
    /// The console driver builder extension methods to make it easier to use <see cref="IConsoleDriverBuilder"/>
    /// </summary>
    public static class ConsoleDrivingAbstractionsConsoleDriverBuilderExtensions
    {
        /// <summary>
        /// Builds and starts the consoleDriver.
        /// </summary>
        /// <param name="consoleDriverBuilder">The <see cref="IConsoleDriverBuilder"/> to start.</param>
        /// <returns>The started <see cref="IConsoleDriver"/>.</returns>
        public static IConsoleDriver Start(this IConsoleDriverBuilder consoleDriverBuilder)
        {
            return consoleDriverBuilder.StartAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Builds and starts the host.
        /// </summary>
        /// <param name="consoleDriverBuilder">The <see cref="IConsoleDriverBuilder"/> to start.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The started <see cref="IConsoleDriver"/>.</returns>
        public static async Task<IConsoleDriver> StartAsync(this IConsoleDriverBuilder consoleDriverBuilder, CancellationToken cancellationToken = default)
        {
            var consoleDriver = consoleDriverBuilder.Build();
            await consoleDriver.StartAsync(cancellationToken);
            return consoleDriver;
        }
    }
}
