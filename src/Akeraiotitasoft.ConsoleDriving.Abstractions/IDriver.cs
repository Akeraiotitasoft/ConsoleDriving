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
    /// The driver interface.
    /// ConsoleDrivingBuilder will run the class that implements <see cref="IDriver"/>
    /// </summary>
    public interface IDriver
    {
        /// <summary>
        /// The entry point of execution
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>A task that contains an integer exit code</returns>
        Task<int> ExecuteAsync(string[] args, CancellationToken cancellationToken = default);
    }
}
