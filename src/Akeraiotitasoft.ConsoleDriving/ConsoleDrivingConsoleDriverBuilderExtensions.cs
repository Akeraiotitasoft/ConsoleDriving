// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

// Licensed to the .NET Foundation under one or more agreements.  (The latest change - original version)
// The .NET Foundation licenses this file to you under the MIT license.  (The latest change - original version)

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

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
        /// Specify the <see cref="IServiceProvider"/> to be the default one.
        /// </summary>
        /// <param name="consoleDriverBuilder">The <see cref="IConsoleDriverBuilder"/> to configure.</param>
        /// <param name="configure"></param>
        /// <returns>The <see cref="IConsoleDriverBuilder"/>.</returns>
        public static IConsoleDriverBuilder UseDefaultServiceProvider(this IConsoleDriverBuilder consoleDriverBuilder, Action<ServiceProviderOptions> configure)
            => consoleDriverBuilder.UseDefaultServiceProvider((context, options) => configure(options));

        /// <summary>
        /// Specify the <see cref="IServiceProvider"/> to be the default one.
        /// </summary>
        /// <param name="consoleDriverBuilder">The <see cref="IConsoleDriverBuilder"/> to configure.</param>
        /// <param name="configure">The delegate that configures the <see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="IConsoleDriverBuilder"/>.</returns>
        public static IConsoleDriverBuilder UseDefaultServiceProvider(this IConsoleDriverBuilder consoleDriverBuilder, Action<ConsoleDriverBuilderContext, ServiceProviderOptions> configure)
        {
            return consoleDriverBuilder.UseServiceProviderFactory(context =>
            {
                var options = new ServiceProviderOptions();
                configure(context, options);
                return new DefaultServiceProviderFactory(options);
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

        /// <summary>
        /// Configures an existing <see cref="IConsoleDriverBuilder"/> instance with pre-configured defaults. This will overwrite
        /// previously configured values and is intended to be called before additional configuration calls.
        /// </summary>
        /// <remarks>
        ///   The following defaults are applied to the <see cref="IConsoleDriverBuilder"/>:
        ///   <list type="bullet">
        ///     <item><description>set the <see cref="IConsoleDrivingEnvironment.ContentRootPath"/> to the result of <see cref="Directory.GetCurrentDirectory()"/></description></item>
        ///     <item><description>load host <see cref="IConfiguration"/> from "DOTNET_" prefixed environment variables</description></item>
        ///     <item><description>load host <see cref="IConfiguration"/> from supplied command line args</description></item>
        ///     <item><description>load app <see cref="IConfiguration"/> from 'appsettings.json' and 'appsettings.[<see cref="IConsoleDrivingEnvironment.EnvironmentName"/>].json'</description></item>
        ///     <item><description>load app <see cref="IConfiguration"/> from User Secrets when <see cref="IConsoleDrivingEnvironment.EnvironmentName"/> is 'Development' using the entry assembly</description></item>
        ///     <item><description>load app <see cref="IConfiguration"/> from environment variables</description></item>
        ///     <item><description>load app <see cref="IConfiguration"/> from supplied command line args</description></item>
        ///     <item><description>configure the <see cref="ILoggerFactory"/> to log to the console, debug, and event source output</description></item>
        ///     <item><description>enables scope validation on the dependency injection container when <see cref="IConsoleDrivingEnvironment.EnvironmentName"/> is 'Development'</description></item>
        ///   </list>
        /// </remarks>
        /// <param name="builder">The existing builder to configure.</param>
        /// <param name="args">The command line args.</param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        public static IConsoleDriverBuilder ConfigureDefaults(this IConsoleDriverBuilder builder, string[] args)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            builder.ConfigureConsoleDriverConfiguration(config =>
            {
                config.AddEnvironmentVariables(prefix: "DOTNET_");
                if (args is { Length: > 0 })
                {
                    config.AddCommandLine(args);
                }
            });

            builder.ConfigureAppConfiguration((consoleDriverContext, config) =>
            {
                IConsoleDrivingEnvironment env = consoleDriverContext.ConsoleDrivingEnvironment;
                bool reloadOnChange = GetReloadConfigOnChangeValue(consoleDriverContext);

                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: reloadOnChange)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: reloadOnChange);

                if (env.IsDevelopment() && env.ApplicationName is { Length: > 0 })
                {
                    var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                    if (appAssembly is not null)
                    {
                        config.AddUserSecrets(appAssembly, optional: true, reloadOnChange: reloadOnChange);
                    }
                }

                config.AddEnvironmentVariables();

                if (args is { Length: > 0 })
                {
                    config.AddCommandLine(args);
                }
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                bool isWindows =
#if NET6_0_OR_GREATER
                    OperatingSystem.IsWindows();
#else
                    RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif

                // IMPORTANT: This needs to be added *before* configuration is loaded, this lets
                // the defaults be overridden by the configuration.
                if (isWindows)
                {
                    // Default the EventLogLoggerProvider to warning or above
                    logging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Warning);
                }

                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
#if NET6_0_OR_GREATER
                if (!OperatingSystem.IsBrowser())
#endif
                {
                    logging.AddConsole();
                }
                logging.AddDebug();
                logging.AddEventSourceLogger();

                if (isWindows)
                {
                    // Add the EventLogLoggerProvider on windows machines
                    logging.AddEventLog();
                }

                logging.Configure(options =>
                {
                    options.ActivityTrackingOptions =
                        ActivityTrackingOptions.SpanId |
                        ActivityTrackingOptions.TraceId |
                        ActivityTrackingOptions.ParentId;
                });

            })
            .UseDefaultServiceProvider((context, options) =>
            {
                bool isDevelopment = context.ConsoleDrivingEnvironment.IsDevelopment();
                options.ValidateScopes = isDevelopment;
                options.ValidateOnBuild = isDevelopment;
            });

            return builder;

            [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Calling IConfiguration.GetValue is safe when the T is bool.")]
            static bool GetReloadConfigOnChangeValue(ConsoleDriverBuilderContext consoleDrivingContext) => consoleDrivingContext.Configuration.GetValue("consoleDrivingBuilder:reloadConfigOnChange", defaultValue: true);
        }
    }
}

