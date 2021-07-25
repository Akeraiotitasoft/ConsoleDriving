// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Akeraiotitasoft.ConsoleDriving.Internal
{
    internal class ConsoleDriver : IConsoleDriver
    {
        //private string[] _arguments;
        private readonly ICommandLineArguments _commandLineArguments;
        private readonly ILogger<ConsoleDriver> _logger;
        //private readonly IHostLifetime _hostLifetime;
        //private readonly ApplicationLifetime _applicationLifetime;
        /*private readonly ConsoleDriverOptions _options;*/
        private IEnumerable<IDriver> _drivers;

        public ConsoleDriver(ICommandLineArguments commandLineArguments, IServiceProvider services, /*IApplicationLifetime applicationLifetime,*/ ILogger<ConsoleDriver> logger
            /*IHostLifetime hostLifetime,*/ /*IOptions<ConsoleDriverOptions> options*/)
        {
            _commandLineArguments = commandLineArguments;
            Services = services ?? throw new ArgumentNullException(nameof(services));
            //_applicationLifetime = (applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime))) as ApplicationLifetime;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //_hostLifetime = hostLifetime ?? throw new ArgumentNullException(nameof(hostLifetime));
            //_options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public IServiceProvider Services { get; }


        public int Result => throw new NotImplementedException();

        public void Dispose()
        {
            (Services as IDisposable)?.Dispose();
        }

        public async Task<int> StartAsync(CancellationToken cancellationToken = default)
        {
            _logger.Starting();

            _drivers = Services.GetService<IEnumerable<IDriver>>();

            int exitCode = 0;

            foreach (var driver in _drivers)
            {
                // Fire IDriver.Execute
                int tempExitCode = await driver.ExecuteAsync(_commandLineArguments.Arguments, cancellationToken).ConfigureAwait(false);

                if (tempExitCode != 0)
                {
                    exitCode = tempExitCode;
                }
            }

            _logger.Stopped();

            return exitCode;
        }
    }
}
