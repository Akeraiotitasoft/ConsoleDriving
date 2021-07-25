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
    /// A program abstraction.
    /// </summary>
    public interface IConsoleDriver : IDisposable
    {
        /// <summary>
        /// The programs configured services.
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// The result
        /// </summary>
        int Result { get; }

        /// <summary>
        /// Start the program.
        /// </summary>
        /// <param name="cancellationToken">Used to abort program start.</param>
        /// <returns>A task with the exit code</returns>
        Task<int> StartAsync(CancellationToken cancellationToken = default);
    }
}
