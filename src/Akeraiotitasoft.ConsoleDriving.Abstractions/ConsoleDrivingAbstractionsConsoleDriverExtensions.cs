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
    /// The console driver extensions to make it easier to use <see cref="IConsoleDriver"/>
    /// </summary>
    public static class ConsoleDrivingAbstractionsConsoleDriverExtensions
    {
        /// <summary>
        /// Starts the consoleDriver synchronously.
        /// </summary>
        /// <param name="consoleDriver"></param>
        /// <returns>The error level exit code</returns>
        public static int Start(this IConsoleDriver consoleDriver)
        {
            return consoleDriver.StartAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Runs an application and block the calling thread until consoleDriver shutdown.
        /// </summary>
        /// <param name="consoleDriver">The <see cref="IConsoleDriver"/> to run.</param>
        /// <returns>The error level exit code</returns>
        public static int Run(this IConsoleDriver consoleDriver)
        {
            return consoleDriver.RunAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Runs an application and returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        /// <param name="consoleDriver">The <see cref="IConsoleDriver"/> to run.</param>
        /// <param name="token">The token to trigger shutdown.</param>
        /// <returns>A task containing the error level exit code</returns>
        public static async Task<int> RunAsync(this IConsoleDriver consoleDriver, CancellationToken token = default)
        {
            using (consoleDriver)
            {
                return await consoleDriver.StartAsync(token);
            }
        }
    }
}
