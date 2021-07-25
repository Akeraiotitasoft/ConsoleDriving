// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Akeraiotitasoft.ConsoleDriving
{
    /// <summary>
    /// Extension methods for console driving.  This is so it is easier to use the console driver builder.
    /// </summary>
    public static class ConsoleDrivingConsoleDriverBuilderExtensions
    {
        /// <summary>
        /// Specify the environment to be used by the console driver.
        /// </summary>
        /// <param name="consoleDriverBuilder">The <see cref="IConsoleDriverBuilder"/> to configure.</param>
        /// <param name="environment">The environment to run the driver for the application in.</param>
        /// <returns>The <see cref="IConsoleDriverBuilder"/>.</returns>
        public static IConsoleDriverBuilder UseEnvironment(this IConsoleDriverBuilder consoleDriverBuilder, string environment)
        {
            return consoleDriverBuilder.ConfigureConsoleDriverConfiguration(configBuilder =>
            {
                configBuilder.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(ConsoleDriverDefaults.EnvironmentKey,
                        environment  ?? throw new ArgumentNullException(nameof(environment)))
                });
            });
        }

        /// <summary>
        /// Specify the content root directory to be used by the console driver.
        /// </summary>
        /// <param name="consoleDriverBuilder">The <see cref="IConsoleDriverBuilder"/> to configure.</param>
        /// <param name="contentRoot">Path to root directory of the application.</param>
        /// <returns>The <see cref="IConsoleDriverBuilder"/>.</returns>
        public static IConsoleDriverBuilder UseContentRoot(this IConsoleDriverBuilder consoleDriverBuilder, string contentRoot)
        {
            return consoleDriverBuilder.ConfigureConsoleDriverConfiguration(configBuilder =>
            {
                configBuilder.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(ConsoleDriverDefaults.ContentRootKey,
                        contentRoot  ?? throw new ArgumentNullException(nameof(contentRoot)))
                });
            });
        }

        /// <summary>
        /// Adds a delegate for configuring the provided <see cref="ILoggingBuilder"/>. This may be called multiple times.
        /// </summary>
        /// <param name="consoleDriverBuilder">The <see cref="IConsoleDriverBuilder" /> to configure.</param>
        /// <param name="configureLogging">The delegate that configures the <see cref="ILoggingBuilder"/>.</param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        public static IConsoleDriverBuilder ConfigureLogging(this IConsoleDriverBuilder consoleDriverBuilder, Action<ConsoleDriverBuilderContext, ILoggingBuilder> configureLogging)
        {
            return consoleDriverBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder => configureLogging(context, builder)));
        }

        /// <summary>
        /// Adds a delegate for configuring the provided <see cref="ILoggingBuilder"/>. This may be called multiple times.
        /// </summary>
        /// <param name="consoleDriverBuilder">The <see cref="IConsoleDriverBuilder" /> to configure.</param>
        /// <param name="configureLogging">The delegate that configures the <see cref="ILoggingBuilder"/>.</param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        public static IConsoleDriverBuilder ConfigureLogging(this IConsoleDriverBuilder consoleDriverBuilder, Action<ILoggingBuilder> configureLogging)
        {
            return consoleDriverBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder => configureLogging(builder)));
        }
        /// <summary>
        /// Sets up the configuration for the remainder of the build process and application. This can be called multiple times and
        /// the results will be additive. The results will be available at <see cref="ConsoleDriverBuilderContext.Configuration"/> for
        /// subsequent operations, as well as in <see cref="IConsoleDriver.Services"/>.
        /// </summary>
        /// <param name="consoleDriverBuilder">The <see cref="IConsoleDriverBuilder" /> to configure.</param>
        /// <param name="configureDelegate"></param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        public static IConsoleDriverBuilder ConfigureAppConfiguration(this IConsoleDriverBuilder consoleDriverBuilder, Action<IConfigurationBuilder> configureDelegate)
        {
            return consoleDriverBuilder.ConfigureAppConfiguration((context, builder) => configureDelegate(builder));
        }

        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="consoleDriverBuilder">The <see cref="IConsoleDriverBuilder" /> to configure.</param>
        /// <param name="configureDelegate"></param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        public static IConsoleDriverBuilder ConfigureServices(this IConsoleDriverBuilder consoleDriverBuilder, Action<IServiceCollection> configureDelegate)
        {
            return consoleDriverBuilder.ConfigureServices((context, collection) => configureDelegate(collection));
        }

        /// <summary>
        /// Enables configuring the instantiated dependency container. This can be called multiple times and
        /// the results will be additive.
        /// </summary>
        /// <typeparam name="TContainerBuilder"></typeparam>
        /// <param name="consoleDriverBuilder">The <see cref="IConsoleDriverBuilder" /> to configure.</param>
        /// <param name="configureDelegate"></param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        public static IConsoleDriverBuilder ConfigureContainer<TContainerBuilder>(this IConsoleDriverBuilder consoleDriverBuilder, Action<TContainerBuilder> configureDelegate)
        {
            return consoleDriverBuilder.ConfigureContainer<TContainerBuilder>((context, builder) => configureDelegate(builder));
        }
    }
}

